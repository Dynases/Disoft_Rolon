USE [BDDistBHF_CF]
GO

/****** Object:  View [dbo].[VR_GO_CajaGeneral]    Script Date: 26/02/2020 10:48:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VR_GO_CajaGeneral]
AS
SELECT        TOP (100) PERCENT caja.olnumi AS IdCaja, caja.olfecha AS FechaCaja, conci.oacnconc AS Conciliacion, pedido.oafdoc AS FechaPedido, repartidor.cbdesc AS Repartidor, SUM(detalle.obptot) AS TotalConciliacion,
                             (SELECT        ISNULL(SUM(omTotalB) + SUM(omTotalD) * caja.olTipoCambio, 0) AS Expr1
                               FROM            dbo.TO0051 AS cambio
                               WHERE        (omolnumi = caja.olnumi)) AS TotalEfectivo, caja.olCredito AS TotalCredito,
                             (SELECT        ISNULL(SUM(onMonto), 0) AS Expr1
                               FROM            dbo.TO0052 AS deposito
                               WHERE        (onolnumi = caja.olnumi)) AS TotalDeposito,
                             (SELECT        ISNULL(SUM(omTotalB) + SUM(omTotalD) * caja.olTipoCambio, 0) AS Expr1
                               FROM            dbo.TO0051 AS cambio
                               WHERE        (omolnumi = caja.olnumi)) + caja.olCredito +
                             (SELECT        ISNULL(SUM(onMonto), 0) AS Expr1
                               FROM            dbo.TO0052 AS deposito
                               WHERE        (onolnumi = caja.olnumi)) AS TotalGeneral, SUM(detalle.obptot) -
                             ((SELECT        ISNULL(SUM(omTotalB) + SUM(omTotalD) * caja.olTipoCambio, 0) AS Expr1
                                 FROM            dbo.TO0051 AS cambio
                                 WHERE        (omolnumi = caja.olnumi)) + caja.olCredito +
                             (SELECT        ISNULL(SUM(onMonto), 0) AS Expr1
                               FROM            dbo.TO0052 AS deposito
                               WHERE        (onolnumi = caja.olnumi))) AS Diferencia
FROM            dbo.TO005 AS caja INNER JOIN
                         dbo.TO001A AS a ON a.oaacaja = caja.olnumi INNER JOIN
                         dbo.TO001 AS pedido ON pedido.oanumi = a.oaato1numi INNER JOIN
                         dbo.TO0011 AS detalle ON detalle.obnumi = pedido.oanumi INNER JOIN
                         dbo.TO001C AS conci ON conci.oacoanumi = pedido.oanumi INNER JOIN
                         dbo.TC002 AS repartidor ON repartidor.cbnumi = conci.oaccbnumi
GROUP BY pedido.oafdoc, pedido.oafdoc, repartidor.cbdesc, conci.oacnconc, caja.olTipoCambio, caja.olnumi, caja.olCredito, caja.olfecha

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "caja"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 263
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "a"
            Begin Extent = 
               Top = 6
               Left = 301
               Bottom = 136
               Right = 526
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "pedido"
            Begin Extent = 
               Top = 6
               Left = 564
               Bottom = 136
               Right = 789
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "repartidor"
            Begin Extent = 
               Top = 6
               Left = 827
               Bottom = 136
               Right = 1052
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "detalle"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 263
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "conci"
            Begin Extent = 
               Top = 138
               Left = 301
               Bottom = 268
               Right = 526
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VR_GO_CajaGeneral'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VR_GO_CajaGeneral'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VR_GO_CajaGeneral'
GO


