USE [BDDistBHF_CF]
GO
/****** Object:  StoredProcedure [dbo].[sp_Mam_VentasCredito]    Script Date: 06/01/2020 06:08:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--drop procedure sp_Mam_TY004
ALTER PROCEDURE [dbo].[sp_Mam_VentasCredito] (@tipo int=-1,@fechaI date=null,@fechaF date=null,@yduact nvarchar(10)='',
@cliente int=-1,@codCredito int=-1,@catPrecio int=-1,@almacen int=-1,@vendedor int=-1)

--TZ0013Type
AS
BEGIN
	DECLARE @newHora nvarchar(5)
	set @newHora=CONCAT(DATEPART(HOUR,GETDATE()),':',DATEPART(MINUTE,GETDATE()))
	declare @numi int
	DECLARE @newFecha date
	set @newFecha=GETDATE()
		IF @tipo=1 --
	BEGIN
		BEGIN TRY 
		select cliente .ccnumi  as numicliente,cliente .ccdesc   as cliente,
(select  Sum(totalcredito .ogcred  ) from TO001A1 as totalcredito,TO001 as aux where aux.oanumi =totalcredito .ognumi   and aux.oaccli  =cliente .ccnumi  
and aux.oafdoc   >=@fechaI  and aux.oafdoc  <=@fechaF )as credito,

isnull((select Sum(aporte.tdmonto )  from TV00121 as aporte ,TO001 as aux where aux.oanumi  =aporte .tdtv12numi 
and aux.oaccli  =cliente .ccnumi  and aux.oafdoc  >=@fechaI and aux.oafdoc  <=@fechaF),0) as aporte

,isnull(((select  Sum(totalcredito .ogcred  ) from TO001A1 as totalcredito,TO001 as aux where 
aux.oanumi =totalcredito .ognumi  and aux .oaccli  =cliente .ccnumi   and aux.oafdoc  >=@fechaI
and aux.oafdoc <=@fechaF)-
(select Sum(aporte.tdmonto )  from TV00121 as aporte ,TO001 as aux where aux.oanumi   =aporte .tdtv12numi 
and aux.oaccli  =cliente .ccnumi  and aux.oafdoc  >=@fechaI and aux .oafdoc  <=@fechaF )),
 (select  Sum(totalcredito .ogcred  ) from TO001A1 as totalcredito,TO001 as aux where 
 aux.oanumi =totalcredito .ognumi  and aux.oaccli  =cliente .ccnumi  
 and aux.oafdoc  >=@fechaI and aux.oafdoc<=@fechaF))as deuda,
a.oanumi  as numiventa,'Almacen Principal' as aabdes ,FORMAT (a.oafdoc , 'dd-MM-yyyy') as fechaventa,FORMAT (a.oafdoc , 'dd-MM-yyyy') as fechacredito,credito .ogcred as tctotcre ,
detallepago .tdnrorecibo ,FORMAT (detallepago .tdfechaPago , 'dd-MM-yyyy') as tdfechaPago ,detallepago .tdnrodoc  ,detallepago .tdmonto 
,IIF((select Sum(auxdetallepago.tdmonto) from TV00121 as auxdetallepago where auxdetallepago.tdtv12numi=a.oanumi)=credito.ogcred 
,IIF((select Max(ayuda.tdnumi) from TV00121 ayuda where ayuda.tdtv12numi=a.oanumi)=detallepago.tdnumi,'CANCELACION TOTAL',
'CANCELACION PARCIAL'),'CANCELACION PARCIAL')as observacion
from TO001  as a 
inner join TC004 as cliente on
 cliente .ccnumi  =a.oaccli  
 inner join TO001A1  as credito on credito.ognumi  =a.oanumi  
left join TV00121 as detallepago on detallepago .tdtv12numi =a.oanumi  
and a.oafdoc >=@fechaI and a.oafdoc  <=@fechaF 
group by cliente .ccnumi ,cliente .ccdesc ,a.oanumi  ,a.oafdoc   ,credito.ogcred ,
detallepago .tdnrorecibo ,detallepago .tdfechaPago ,detallepago .tdnrodoc  ,detallepago .tdmonto ,detallepago .tdnumi 
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),1,@newFecha,@newHora,@yduact)
		END CATCH
	END

			IF @tipo=3 --
	BEGIN
		BEGIN TRY 
		select cliente .ccnumi  as numicliente,cliente .ccdesc   as cliente,
(select  Sum(totalcredito .ogcred  ) from TO001A1 as totalcredito,TO001 as aux where aux.oanumi =totalcredito .ognumi   and aux.oaccli  =cliente .ccnumi  
and aux.oafdoc   >=@fechaI  and aux.oafdoc  <=@fechaF )as credito,

isnull((select Sum(aporte.tdmonto )  from TV00121 as aporte ,TO001 as aux where aux.oanumi  =aporte .tdtv12numi 
and aux.oaccli  =cliente .ccnumi  and aux.oafdoc  >=@fechaI and aux.oafdoc  <=@fechaF),0) as aporte

,isnull(((select  Sum(totalcredito .ogcred  ) from TO001A1 as totalcredito,TO001 as aux where 
aux.oanumi =totalcredito .ognumi  and aux .oaccli  =cliente .ccnumi   and aux.oafdoc  >=@fechaI
and aux.oafdoc <=@fechaF)-
(select Sum(aporte.tdmonto )  from TV00121 as aporte ,TO001 as aux where aux.oanumi   =aporte .tdtv12numi 
and aux.oaccli  =cliente .ccnumi  and aux.oafdoc  >=@fechaI and aux .oafdoc  <=@fechaF )),
 (select  Sum(totalcredito .ogcred  ) from TO001A1 as totalcredito,TO001 as aux where 
 aux.oanumi =totalcredito .ognumi  and aux.oaccli  =cliente .ccnumi  
 and aux.oafdoc  >=@fechaI and aux.oafdoc<=@fechaF))as deuda,
a.oanumi  as numiventa,'Almacen Principal' as aabdes ,FORMAT (a.oafdoc , 'dd-MM-yyyy') as fechaventa,FORMAT (a.oafdoc , 'dd-MM-yyyy') as fechacredito,credito .ogcred as tctotcre  ,
detallepago .tdnrorecibo ,FORMAT (detallepago .tdfechaPago , 'dd-MM-yyyy') as tdfechaPago ,detallepago .tdnrodoc  ,detallepago .tdmonto 
,IIF((select Sum(auxdetallepago.tdmonto) from TV00121 as auxdetallepago where auxdetallepago.tdtv12numi=a.oanumi)=credito.ogcred 
,IIF((select Max(ayuda.tdnumi) from TV00121 ayuda where ayuda.tdtv12numi=a.oanumi)=detallepago.tdnumi,'CANCELACION TOTAL',
'CANCELACION PARCIAL'),'CANCELACION PARCIAL')as observacion
from TO001  as a 
inner join TC004 as cliente on
 cliente .ccnumi  =a.oaccli  
 inner join TO001A1  as credito on credito.ognumi  =a.oanumi  
left join TV00121 as detallepago on detallepago .tdtv12numi =a.oanumi  
and a.oafdoc >=@fechaI and a.oafdoc  <=@fechaF and cliente .ccnumi  =@cliente 
group by cliente .ccnumi ,cliente .ccdesc ,a.oanumi  ,a.oafdoc   ,credito.ogcred ,
detallepago .tdnrorecibo ,detallepago .tdfechaPago ,detallepago .tdnrodoc  ,detallepago .tdmonto ,detallepago .tdnumi 
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),1,@newFecha,@newHora,@yduact)
		END CATCH
	END

		IF @tipo=5 --
	BEGIN
		BEGIN TRY 
		select cliente .ccnumi  as numicliente,cliente .ccdesc   as cliente,
(select  Sum(totalcredito .ogcred  ) from TO001A1 as totalcredito,TO001 as aux where aux.oanumi =totalcredito .ognumi   and aux.oaccli  =cliente .ccnumi  
and aux.oafdoc   >=@fechaI  and aux.oafdoc  <=@fechaF )as credito,

isnull((select Sum(aporte.tdmonto )  from TV00121 as aporte ,TO001 as aux where aux.oanumi  =aporte .tdtv12numi 
and aux.oaccli  =cliente .ccnumi  and aux.oafdoc  >=@fechaI and aux.oafdoc  <=@fechaF),0) as aporte

,isnull(((select  Sum(totalcredito .ogcred  ) from TO001A1 as totalcredito,TO001 as aux where 
aux.oanumi =totalcredito .ognumi  and aux .oaccli  =cliente .ccnumi   and aux.oafdoc  >=@fechaI
and aux.oafdoc <=@fechaF)-
(select Sum(aporte.tdmonto )  from TV00121 as aporte ,TO001 as aux where aux.oanumi   =aporte .tdtv12numi 
and aux.oaccli  =cliente .ccnumi  and aux.oafdoc  >=@fechaI and aux .oafdoc  <=@fechaF )),
 (select  Sum(totalcredito .ogcred  ) from TO001A1 as totalcredito,TO001 as aux where 
 aux.oanumi =totalcredito .ognumi  and aux.oaccli  =cliente .ccnumi  
 and aux.oafdoc  >=@fechaI and aux.oafdoc<=@fechaF))as deuda,
a.oanumi  as numiventa,'Almacen Principal' as aabdes ,FORMAT (a.oafdoc , 'dd-MM-yyyy') as fechaventa,FORMAT (a.oafdoc , 'dd-MM-yyyy') as fechacredito,credito .ogcred as tctotcre  ,
detallepago .tdnrorecibo ,FORMAT (detallepago .tdfechaPago , 'dd-MM-yyyy') as tdfechaPago ,detallepago .tdnrodoc  ,detallepago .tdmonto 
,IIF((select Sum(auxdetallepago.tdmonto) from TV00121 as auxdetallepago where auxdetallepago.tdtv12numi=a.oanumi)=credito.ogcred 
,IIF((select Max(ayuda.tdnumi) from TV00121 ayuda where ayuda.tdtv12numi=a.oanumi)=detallepago.tdnumi,'CANCELACION TOTAL',
'CANCELACION PARCIAL'),'CANCELACION PARCIAL')as observacion
from TO001  as a 
inner join TC004 as cliente on
 cliente .ccnumi  =a.oaccli  
 inner join TO001A1  as credito on credito.ognumi  =a.oanumi  
left join TV00121 as detallepago on detallepago .tdtv12numi =a.oanumi  
and a.oafdoc >=@fechaI and a.oafdoc  <=@fechaF and a.oanumi  =@codCredito
group by cliente .ccnumi ,cliente .ccdesc ,a.oanumi  ,a.oafdoc   ,credito.ogcred ,
detallepago .tdnrorecibo ,detallepago .tdfechaPago ,detallepago .tdnrodoc  ,detallepago .tdmonto ,detallepago .tdnumi 
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),1,@newFecha,@newHora,@yduact)
		END CATCH
	END

	IF @tipo=111 -- Kardex de Cliente Resumen PR_KardexCredito
	BEGIN
		BEGIN TRY 
	select Isnull(sum(pagos.tdmonto),0) as pago,cliente.ccnumi  as numicliente,cliente .ccdesc   as cliente,
		       cliente.ccdirec  AS direc, cliente.cctelf1  AS telf1, cliente.cctelf2  AS contacto, 
			   venta.oarepa  AS codven, 0 as ydlcred, venta.oanumi   as tanumi, venta.oafdoc   as tafdoc, venta.oafdoc as  tafvcr,
			   0 as yddias,(select Sum(detalle .obptot ) from TO0011 as detalle where detalle .obnumi =venta.oanumi ) as tatotal,
               (SELECT       cbdesc   as yddesc
                  FROM            TC002
                 WHERE        cbnumi   = venta.oarepa ) AS desven,
				cred.ogcred  as TotCredit,isnull((cred.ogcred-(select Sum(detalle.tdmonto )  from TV00121 as detalle where detalle.tdtv12numi =venta.oanumi )),
				(select Sum(detalle .obptot ) from TO0011 as detalle where detalle .obnumi =venta.oanumi )) as pendiente 
		FRom TC004 as cliente, TO001 as venta, TO001A1 as cred LEFT OUTER JOIN tv00121 pagos on cred .ognumi  = pagos.tdtv12numi
		where venta.oaccli  = cliente.ccnumi 
		  and venta.oanumi  = cred.ognumi 
		  and venta.oafdoc  >= @fechaI
		  and venta.oafdoc  <= @fechaF
		  and cred.ogcred >0
		  group by cliente.ccnumi ,cliente .ccdesc ,
		       cliente.ccdirec , cliente.cctelf1 , cliente.cctelf2 , 
			   venta.oanumi, venta.oafdoc ,venta.oarepa ,cred.ogcred 
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),1,@newFecha,@newHora,@yduact)
		END CATCH
	END

	
	IF @tipo=112 -- Kardex de Cliente Resumen PR_KardexCredito
	BEGIN
		BEGIN TRY 
	select Isnull(sum(pagos.tdmonto),0) as pago,cliente.ccnumi  as numicliente,cliente .ccdesc   as cliente,
		       cliente.ccdirec  AS direc, cliente.cctelf1  AS telf1, cliente.cctelf2  AS contacto, 
			   venta.oarepa  AS codven, 0 as ydlcred, venta.oanumi   as tanumi, venta.oafdoc   as tafdoc, venta.oafdoc as  tafvcr,
			   0 as yddias,(select Sum(detalle .obptot ) from TO0011 as detalle where detalle .obnumi =venta.oanumi ) as tatotal,
               (SELECT       cbdesc   as yddesc
                  FROM            TC002
                 WHERE        cbnumi   = venta.oarepa ) AS desven,
				cred.ogcred  as TotCredit,isnull((cred.ogcred-(select Sum(detalle.tdmonto )  from TV00121 as detalle where detalle.tdtv12numi =venta.oanumi )),
				(select Sum(detalle .obptot ) from TO0011 as detalle where detalle .obnumi =venta.oanumi )) as pendiente 
		FRom TC004 as cliente, TO001 as venta, TO001A1 as cred LEFT OUTER JOIN tv00121 pagos on cred .ognumi  = pagos.tdtv12numi
		where venta.oaccli  = cliente.ccnumi 
		  and venta.oanumi  = cred.ognumi 
		  and venta.oafdoc  >= @fechaI
		  and venta.oafdoc  <= @fechaF
		   and venta.oaccli  = @cliente
		 group by cliente.ccnumi ,cliente .ccdesc ,
		       cliente.ccdirec , cliente.cctelf1 , cliente.cctelf2 , 
			   venta.oanumi, venta.oafdoc ,venta.oarepa ,cred.ogcred 
		END TRY
		BEGIN CATCH
			INSERT INTO TB001 (banum,baproc,balinea,bamensaje,batipo,bafact,bahact,bauact)
				   VALUES(ERROR_NUMBER(),ERROR_PROCEDURE(),ERROR_LINE(),ERROR_MESSAGE(),1,@newFecha,@newHora,@yduact)
		END CATCH
	END
End


