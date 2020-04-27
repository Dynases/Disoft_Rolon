USE [BDDistBHF_CF]
GO

/****** Object:  StoredProcedure [dbo].[sp_go_TI005]    Script Date: 21/02/2020 18:50:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[sp_go_TI005](@tipo int, @id int=-1, @fdoc date=null, @concep int=-1, @obs nvarchar(100)='', @codpack int=-1,
									@cantP int=-1, @pcosto decimal(18,4)=0, @cantNP int=-1, @est int=-1, @alm int=-1, @uact nvarchar(10)='',
									@TI0051 dbo.TI0051Type Readonly)
AS
BEGIN
	DECLARE @newHora nvarchar(5)
	set @newHora=CONCAT(DATEPART(HOUR,GETDATE()),':',DATEPART(MINUTE,GETDATE()))

	DECLARE @newFecha date
	set @newFecha=GETDATE()
	
	IF @tipo=-1 --ELIMINAR REGISTRO
	BEGIN
		BEGIN TRAN ELIMINAR 
		BEGIN TRY 
			DELETE FROM TI0021 WHERE icibid=@id
			DELETE FROM TI002 WHERE ibid=@id
			SELECT @id AS newNumi
			COMMIT TRAN ELIMINAR
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), -1, @newFecha, @newHora, @uact)
			ROLLBACK TRAN ELMINAR
		END CATCH
	END

	IF @tipo=1 --NUEVO REGISTRO
	BEGIN
		BEGIN TRAN INSERTAR
		BEGIN TRY 
			set @id=IIF((select COUNT(igid) from TI005)= 0, 0, (select MAX(igid) from TI005))+1

			INSERT INTO TI005 VALUES(@id, @fdoc, @obs, @codpack, @cantP, @pcosto, @cantNP, @est, @alm, @newFecha, @newHora, @uact)
			
			-- INSERTO EL DETALLE 
			INSERT INTO TI0051(ihigid, ihcodpro, ihcant, ihtotcant, ihpcosto, ihfact, ihhact, ihuact)
			SELECT @id, td.ihcodpro, td.ihcant, td.ihtotcant, td.ihpcosto, @newFecha, @newHora, td.ihuact
			FROM @TI0051 AS td
			WHERE td.ihcodpro>0;

			-- DEVUELVO VALORES DE CONFIRMACION
			SELECT @id AS newNumi
			COMMIT TRAN INSERTAR
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), 1, @newFecha, @newHora, @uact)

			ROLLBACK TRAN INSERTAR
		END CATCH
	END
	
	IF @tipo=2--MODIFICACION
	BEGIN
		BEGIN TRAN MODIFICACION
		BEGIN TRY 
			--UPDATE TI002 SET ibfdoc=@fdoc, ibconcep=@concep, ibobs=@obs, ibest=@est, ibalm=@alm, ibiddc=@iddc,
			--				 ibfact=@newFecha, ibhact=@newHora, ibuact=@uact
			--		 Where ibid=@id;

			----MODIFICO EL DETALLE
			--DELETE FROM TI0021 WHERE TI0021.icid in (SELECT a.icid 
			--										 FROM TI0021 a left join @TI0021 td on a.icid=td.icid and a.icibid=@id
			--										 WHERE td.icid is null) and TI0021.icibid=@id;

			--INSERT INTO TI0021(icibid, iccprod, iccant) SELECT @id, td.iccprod, td.iccant
			--											FROM @TI0021 AS td
			--											WHERE td.estado=0 and td.iccprod>0;

			--UPDATE TI0021 SET TI0021.iccprod=td.iccprod, TI0021.iccant=td.iccant
			--			  FROM TI0021 INNER JOIN @TI0021 AS td ON TI0021.icid=td.icid and td.estado=2;

			----DEVUELVO VALORES DE CONFIRMACION
			--select @id as newNumi
			COMMIT TRAN MODIFICACION
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), 2, @newFecha, @newHora, @uact)
			ROLLBACK TRAN MODIFICACION
		END CATCH
	END

	IF @tipo=3 --MOSTRAR TODOS LOS MOVIMIENTOS
	BEGIN
		BEGIN TRY
			SELECT a.ibid as id, a.ibfdoc as fdoc, a.ibconcep as concep, b.cpdesc as nconcep, a.ibobs as obs, 
				   a.ibest as est, a.ibalm as alm, a.ibiddc as iddc, 
				   a.ibfact as fact, a.ibhact as hact, a.ibuact as uact 
			FROM TI002 a inner join TCI001 b on a.ibconcep=b.cpnumi and a.ibest=@est
			ORDER BY ibid
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), 3, @newFecha, @newHora, @uact)
		END CATCH
	END

	IF @tipo=4 --MOSTRAR TODOS DETALLE DE MOVIMIENTO
	BEGIN
		BEGIN TRY
			SELECT a.ihigid, a.ihcodpro, b.cadesc, a.ihcant, a.ihtotcant, a.ihpcosto, 0 as stock, 1 as estado 
			FROM TI0051 a inner join TC001 b on a.ihcodpro=b.canumi and a.ihigid=@id
			
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), 4, @newFecha, @newHora, @uact)
		END CATCH
	END

	IF @tipo=5 --MOSTRAR PRODUCTOS PACK
	BEGIN
		BEGIN TRY
			SELECT a.cbnumi, a.cbtccanumi, a.cbtccanumi1, b.cadesc, a.cbcant, 1 as estado
			FROM TC0013 a
			inner join TC001 b on a.cbtccanumi1=b.canumi
			where a.cbtccanumi=@id
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum, baproc, balinea, bamensaje, batipo, bafact, bahact, bauact)
				   VALUES(ERROR_NUMBER(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), 5, @newFecha, @newHora, @uact)
		END CATCH

	END

END




GO


