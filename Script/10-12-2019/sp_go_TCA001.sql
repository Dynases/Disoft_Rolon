USE [BDDistBHF_CF]
GO
/****** Object:  StoredProcedure [dbo].[sp_go_TCA001]    Script Date: 10/12/2019 07:12:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[sp_go_TCA001](@tipo int, @numi int=-1, @fdoc date=null, @prov int=-1, @nfac nvarchar(20)='',
								    @obs nvarchar(100)='', @uact nvarchar(10)='', @TCA0011 dbo.TCA0011Type Readonly,
									@tven int=-1 , @fvcred date=null, @mon int=-1, @est int=-1, @desc decimal(18,2)=0,
									@total decimal(18,2)=0, @emision int=-1, @consigna int=-1,  @retenc int=-1,  @asientoi int=-1,
									@TFC001 dbo.TFC001Type Readonly)

AS
BEGIN
	DECLARE @newHora nvarchar(5)
	set @newHora=CONCAT(DATEPART(HOUR,GETDATE()),':',DATEPART(MINUTE,GETDATE()))

	DECLARE @newFecha date
	set @newFecha=GETDATE()

	declare @numi2 int
	declare @numicat int
	declare @nsacf decimal(18,2)
	declare @nomcliprov nvarchar(200)
	declare @contabilizo int
	declare @maxid1 int

	IF @tipo=-1 --ELIMINAR REGISTRO
	BEGIN
		BEGIN TRY 
			update TCA001 set caaest=-1 where caanumi  =@numi
			--DELETE FROM TCA001 WHERE caanumi=@numi
			--DELETE FROM TCA0011 WHERE cabtca1numi=@numi			
			DELETE from BDDiconCF.dbo.TFC001  where fcanumito11=@numi

			-- DELETE EN LA TABLA (TI002) DE KARDEX
			set @numi2 = (select a.ibid from TI002 a where a.ibest=13 and a.ibconcep=8 and a.ibobs like (concat('I-',@numi,'-%')))
			
			delete TI0021 where TI0021.icibid=@numi2
			delete TI002 where ibest=13 and ibconcep=8 and ibobs like (concat('I-',@numi,'-%')) 

			-----Inserto con Estado 3 "Eliminado/Anulado" en BDDiconCF.dbo.TPA001 que servirá para hacer el asiento contable-----
			set @contabilizo=(select count(*) 
			from BDDiconCF.dbo.TPA001 as a where a.aanumipadre=@numi and aanumiasiento>0 and (aatipo=1 or aatipo=5))

			if 	@contabilizo=1		
				Begin
				INSERT INTO BDDiconCF.dbo.TPA001
				SELECT aanumipadre, aafecha,1, aacodcliprov, aanomcliprov, aaemision,3,aamontototal, aamoneda,6.96, aanscf, -2
				FROM BDDiconCF.dbo.TPA001  WHERE aanumi in 
				(SELECT max(aanumi) FROM BDDiconCF.dbo.TPA001 WHERE aanumipadre=@numi and (aatipo=1 or aatipo=5))
			End
			else
				Begin
				INSERT INTO BDDiconCF.dbo.TPA001
				SELECT aanumipadre, aafecha,1, aacodcliprov, aanomcliprov,aaemision,3,aamontototal, aamoneda,6.96, aanscf, -1
				FROM BDDiconCF.dbo.TPA001  WHERE aanumi in 
				(SELECT max(aanumi) FROM BDDiconCF.dbo.TPA001 WHERE aanumipadre=@numi and (aatipo=1 or aatipo=5))
				----Actualizo el aanumiasiento a -1 de los demás registros que corresponden al aanumipadre que se está eliminando----	
				UPDATE BDDiconCF.dbo.TPA001 SET aanumiasiento=-1
				where (aanumipadre=@numi and (aatipo=1 or aatipo=5)) and (aaestado=1 or aaestado=2)		
			End

			SELECT @numi AS newNumi
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), -1, @newFecha, @newHora, @uact)
		END CATCH
	END

	IF @tipo=1 --NUEVO REGISTRO
	BEGIN
		BEGIN TRAN INSERTAR
		BEGIN TRY 
			set @numi=IIF((select COUNT(caanumi) from TCA001)= 0, 0, (select MAX(caanumi) from TCA001))+1

			INSERT INTO TCA001 VALUES(@numi, @fdoc, @prov, @nfac, @obs, @newFecha, @newHora, @uact,
						@tven, @fvcred, @mon, @est, @desc, @total, @emision, @consigna, @retenc, @asientoi)

			-- INSERTAR EN LA TABLA (TI002) DE KARDEX
			set @numi2=IIF((select COUNT(ibid) from TI002)= 0, 0, (select MAX(ibid) from TI002))+1

			insert into TI002 values(@numi2, @fdoc, 8, concat('I-',@numi,'-COMPRA'), 13, 1, 0, @newFecha, @newHora, @uact)
			
			insert into TI0021(icibid, iccprod, iccant)
				select @numi2, td.cabtc1numi, td.cabcant
				from @TCA0011 as td
				where td.cabtc1numi<>0;

			-- INSERTO EL DETALLE 
			insert into TCA0011(cabtc1numi, cabcant, cabpcom, cabsubtot,cabporc,cabdesc,cabtot, cabputi, cabpven, cabnfr,
			cabstocka, cabstockf, cabtca1numi)
			select td.cabtc1numi, td.cabcant, td.cabpcom, td.cabsubtot, td.cabporc, td.cabdesc, td.cabtot, td.cabputi, 
			td.cabpven, td.cabnfr, td.cabstocka, td.cabstockf, @numi
			from @TCA0011 AS td
			where td.cabtc1numi<>0;

			------MODIFICO PRECIOS COSTOS------------------
			
			set @numicat =(select Min(cinumi) from TC007 where citcv =0)
			update TC003 set TC003.chprecio =td.cabpcom 
			from TC003 INNER JOIN @TCA0011 AS td
			ON  TC003.chcatcl=@numicat and TC003.chcprod =td.cabtc1numi 
			and td.estado =0 and  td.cabtc1numi > 0 

			--------INSERTAR EN LA TABLA TCA0012 LAS COMPRAS AL CRÉDITO------------------
			if @tven=0
			Begin
				set @maxid1 = iif((select COUNT(a.tcnumi) from TCA0012 a) = 0, 0, (select max(a.tcnumi) from TCA0012 a))
				insert into TCA0012 values(@maxid1+1 ,@numi ,@prov ,@fvcred ,'0','0',@total ,@fdoc ,@newFecha, @newHora, @uact)
			End
			-----Inserto Detalle de Compras en la BDDiconDinoEco.dbo.TFC001-----
			if @emision=1
			Begin				
				INSERT INTO BDDiconCF.dbo.TFC001 (fcafdoc,fcanit,fcarsocial,fcanfac,fcandui,fcaautoriz,
				fcaitc, fcanscf,fcasubtotal, fcadesc,fcaibcf,fcacfiscal,fcaccont,fcatcom,fcanumito11)
				SELECT td.fcafdoc,td.fcanit,td.fcarsocial,td.fcanfac,td.fcandui,td.fcaautoriz,td.fcaitc,
				td.fcanscf,td.fcasubtotal,td.fcadesc, td.fcaibcf,td.fcacfiscal,td.fcaccont,td.fcatcom, @numi FROM @TFC001 AS td;
			End

			-----Inserto con Estado 1 "Vigente" en BDDiconDinoEco.dbo.TPA001 que servirá para hacer el asiento contable-----
			set @nsacf = (SELECT td.fcanscf FROM @TFC001 AS td)			
			set @nomcliprov = (select tc.cmdesc  from TC010 as tc where tc.cmnumi=@prov)
				
			if @consigna=0 and @retenc=0
			Begin	
			INSERT INTO BDDiconCF.dbo.TPA001 values ( @numi,@fdoc,1,@prov,@nomcliprov,@emision,1,@total,@mon,6.96, @nsacf, 0)
			End
			else
				Begin
					if @consigna=0 and @retenc=1			
					INSERT INTO BDDiconCF.dbo.TPA001 values ( @numi,@fdoc,5,@prov,@nomcliprov,@emision,1,@total,@mon,6.96, @nsacf, 0)
				End
			

			-- DEVUELVO VALORES DE CONFIRMACION
			SELECT @numi AS newNumi
			COMMIT TRAN
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), 1, @newFecha, @newHora, @uact)

			ROLLBACK TRAN
		END CATCH
	END
	
	IF @tipo=2--MODIFICACION
	BEGIN
		BEGIN TRAN MODIFICACION
		BEGIN TRY 
		
			UPDATE TCA001 SET caafdoc=@fdoc, caaprov=@prov, caanfac=@nfac, caaobs=@obs, caafact=@newFecha, 
			                  caahact=@newHora, caauact=@uact, caatven=@tven, caafvcred=@fvcred, caamon=@mon,
							  caadesc=@desc, caatotal=@total, caaemision=@emision, caaconsigna=@consigna,
							  caaretenc=@retenc, caaasientoi= @asientoi
			Where caanumi=@numi

			-- UPDATE EN LA TABLA (TI002) DE KARDEX
			update TI002 set ibfdoc=@fdoc, ibfact=@newFecha, ibhact=@newHora, ibuact=@uact
				where ibest=13 and ibconcep=8 and ibobs like (concat('I-',@numi,'-%')) 

			set @numi2 = (select a.ibid from TI002 a where a.ibest=13 and a.ibconcep=8 and a.ibobs like (concat('I-',@numi,'-%')))

			delete TI0021 where TI0021.icibid=@numi2

			insert into TI0021(icibid, iccprod, iccant)
				select @numi2, td.cabtc1numi, td.cabcant
				from @TCA0011 as td
				where td.cabtc1numi<>0;

			----------MODIFICO EL DETALLE------------
			DELETE FROM TCA0011 WHERE TCA0011.cabnumi in (SELECT a.cabnumi 
													  FROM TCA0011 a left join @TCA0011 AS td on a.cabnumi=td.cabnumi and a.cabtca1numi=@numi
													  WHERE td.cabnumi is null) and TCA0011.cabtca1numi=@numi

			INSERT INTO TCA0011(cabtc1numi, cabcant, cabpcom, cabsubtot, cabporc, cabdesc,cabtot, cabputi, cabpven, cabnfr, cabstocka, cabstockf, cabtca1numi)
			SELECT td.cabtc1numi, td.cabcant, td.cabpcom, td.cabsubtot,td.cabporc,td.cabdesc,td.cabtot, td.cabputi, td.cabpven, td.cabnfr, td.cabstocka, td.cabstockf, @numi
				FROM @TCA0011 AS td 
				WHERE td.cabtc1numi<>0 and td.estado=0;

			UPDATE TCA0011
			SET TCA0011.cabtc1numi=td.cabtc1numi, TCA0011.cabcant=td.cabcant, TCA0011.cabpcom=td.cabpcom, TCA0011.cabsubtot=td.cabsubtot, 
			TCA0011.cabporc=td.cabporc, TCA0011.cabdesc=td.cabdesc, TCA0011.cabtot=td.cabtot, TCA0011.cabputi=td.cabputi, 
			TCA0011.cabpven=td.cabpven, TCA0011.cabnfr=td.cabnfr, TCA0011.cabstocka=td.cabstocka, TCA0011.cabstockf=td.cabstockf
			FROM TCA0011 INNER JOIN @TCA0011 AS td ON TCA0011.cabnumi = td.cabnumi and td.estado=2;

			------Modifico tabla facturación de compras en la BDDiconCF.dbo.TFC001--------
			UPDATE BDDiconCF.dbo.TFC001
			SET fcafdoc= td.fcafdoc,fcanit=td.fcanit,fcarsocial=td.fcarsocial,fcanfac=td.fcanfac,fcandui=td.fcandui,fcaautoriz=td.fcaautoriz,
			fcaitc=td.fcaitc,fcanscf=td.fcanscf,fcasubtotal=td.fcasubtotal,fcadesc=td.fcadesc,fcaibcf=td.fcaibcf,fcacfiscal=td.fcacfiscal,
			fcaccont=td.fcaccont,fcatcom=td.fcatcom,fcanumito11= @numi
			FROM @TFC001 AS td
			where BDDiconCF.dbo.TFC001.fcanumito11 = @numi		

			-----Inserto con Estado 2 "Modificado" en BDDiconCF.dbo.TPA001 que servirá para hacer el asiento contable-----
			set @nsacf = (SELECT td.fcanscf FROM @TFC001 AS td)			
			set @nomcliprov = (select tc.cmdesc  from TC010 as tc where tc.cmnumi=@prov)
			
			if @consigna=0 and @retenc=0
			Begin			
			UPDATE BDDiconCF.dbo.TPA001 SET aanumiasiento=-1
			where (aanumipadre=@numi and aatipo=1) and (aaestado=1 or aaestado=2)	

			INSERT INTO BDDiconCF.dbo.TPA001 values (@numi,@fdoc,1,@prov,@nomcliprov,@emision,2,@total,@mon,6.96, @nsacf, 0)
			End

			if @consigna=0 and @retenc=1
			Begin
			UPDATE BDDiconCF.dbo.TPA001 SET aanumiasiento=-1
			where (aanumipadre=@numi and aatipo=5) and (aaestado=1 or aaestado=2)	

			INSERT INTO BDDiconCF.dbo.TPA001 values (@numi,@fdoc,5,@prov,@nomcliprov,@emision,2,@total,@mon,6.96, @nsacf, 0)
			End
			
			if @consigna=1
			Begin
				Delete from BDDiconCF.dbo.TPA001 Where  aanumipadre=@numi and (aatipo=1 or aatipo=5)	
				DELETE from BDDiconCF.dbo.TFC001  where fcanumito11=@numi						
			End

			--DEVUELVO VALORES DE CONFIRMACION
			select @numi as newNumi
			COMMIT TRAN
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), 2, @newFecha, @newHora, @uact)
			ROLLBACK TRAN
		END CATCH
	END

	IF @tipo=3 --MOSTRAR TODOS LAS COMPRAS
	BEGIN
		BEGIN TRY
			SELECT a.caanumi, a.caafdoc, a.caaprov, b.cmdesc as nprov, b.cmnit, a.caanfac, a.caaobs, 
				   a.caafact, a.caahact, a.caauact, a.caatven, a.caafvcred, a.caamon, a.caaest,
				   a.caadesc, a.caatotal, a.caaemision, a.caaconsigna,a.caaasientoi,(select top 1 c.aanumiasiento  from BDDiconCF .dbo.TPA001 as c where c.aanumipadre =a.caanumi and c.aatipo =1 )as asiento
			FROM TCA001 a inner join TC010 b on a.caaprov=b.cmnumi and a.caaest>0
			ORDER BY a.caanumi ASC
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), 3, @newFecha, @newHora, @uact)
		END CATCH
	END

	IF @tipo=4 --OBTENER DETALLE DE LA COMPRA
	BEGIN
		BEGIN TRY

			SELECT a.cabnumi, a.cabtc1numi,b.cacod, b.cadesc as ntc1numi, a.cabcant, a.cabpcom, a.cabputi, a.cabpven, a.cabnfr,
				   a.cabstocka, a.cabstockf, a.cabtca1numi, (a.cabcant*a.cabpcom) as total, a.cabsubtot, a.cabporc, a.cabdesc,
				   a.cabtot, 1 as estado
				FROM TCA0011 a INNER JOIN TC001 b ON a.cabtc1numi=b.canumi and a.cabtca1numi=@numi
			ORDER BY a.cabnumi ASC
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), 4, @newFecha, @newHora, @uact)
		END CATCH
	END

	IF @tipo=5 --Obtener todos pero filtrado BDDiconCF.dbo.TFC001
	BEGIN		
		select fcanumi,fcafdoc,fcanit,fcarsocial,fcanfac,fcandui,fcaautoriz,fcaitc,fcanscf,fcasubtotal,fcadesc,fcaibcf,
		fcacfiscal,fcaccont,fcatcom,fcanumito11,1 as estado
		from BDDiconCF.dbo.TFC001
		where fcanumito11=@numi	
	END

	IF @tipo=6 ------Verificar Pagos
	BEGIN
		BEGIN TRY			
			Select tdnumi, tdtc12numi, tdnrodoc, tdfechaPago, tdmonto, a.tctc1numi,a.tctotcre
			From TCA00121
			inner join TCA0012 as a on a.tcnumi=tdtc12numi and a.tctc1numi=@numi
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), 4, @newFecha, @newHora, @uact)
		END CATCH
	END
	IF @tipo=7 --Verificar si la compra ya fue contabilizada
	BEGIN	
		BEGIN TRY	
			select *
			from BDDiconCF.dbo.TPA001 as a 
			where a.aanumipadre=@numi and  aanumiasiento>0  and (aatipo=1 or aatipo=5)	
		END TRY
		BEGIN CATCH	
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), 4, @newFecha, @newHora, @uact)
		END CATCH
	END

END


