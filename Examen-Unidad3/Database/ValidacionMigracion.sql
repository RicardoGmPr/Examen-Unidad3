-- ============================================================
-- VALIDACIÓN DE INTEGRIDAD DE DATOS - SISTEMA CARL'S JR POS
-- Migración de archivos planos (TXT/JSON) a SQLite
-- Norma ISO/IEC 25024:2015
-- ============================================================

-- ============================================================
-- 1. CONTEO GENERAL DE REGISTROS
-- ============================================================

-- Total de cajeros migrados
SELECT COUNT(*) AS total_cajeros FROM Cajeros WHERE Activo = 1;
-- Resultado esperado: Debe coincidir con líneas en cajeros.txt

-- Total de productos en inventario
SELECT COUNT(*) AS total_productos FROM Inventario;
-- Resultado esperado: 31 productos (7 congelados + 7 refrigerados + 17 secos)

-- Total de tickets procesados
SELECT COUNT(*) AS total_tickets FROM Tickets;

-- Productos por categoría
SELECT Categoria, COUNT(*) AS total
FROM Inventario
GROUP BY Categoria;
-- Esperado: Congelado=7, Refrigerado=7, Seco=17

-- ============================================================
-- 2. VALIDACIÓN DE DUPLICADOS
-- ============================================================

-- Verificar claves de cajeros duplicadas
SELECT Clave, COUNT(*) AS repeticiones
FROM Cajeros
WHERE Activo = 1
GROUP BY Clave
HAVING COUNT(*) > 1;
-- Resultado esperado: 0 filas (no debe haber duplicados)

-- Verificar productos duplicados en inventario
SELECT Nombre, COUNT(*) AS repeticiones
FROM Inventario
GROUP BY Nombre
HAVING COUNT(*) > 1;
-- Resultado esperado: 0 filas

-- Verificar números de ticket duplicados
SELECT NumeroTicket, COUNT(*) AS repeticiones
FROM Tickets
GROUP BY NumeroTicket
HAVING COUNT(*) > 1;
-- Resultado esperado: 0 filas

-- ============================================================
-- 3. VALIDACIÓN DE NULOS INESPERADOS
-- ============================================================

-- Cajeros sin nombre (campo obligatorio)
SELECT COUNT(*) AS cajeros_sin_nombre
FROM Cajeros
WHERE Nombre IS NULL OR Nombre = '';
-- Resultado esperado: 0

-- Cajeros sin clave (campo obligatorio)
SELECT COUNT(*) AS cajeros_sin_clave
FROM Cajeros
WHERE Clave IS NULL OR Clave = '';
-- Resultado esperado: 0

-- Productos sin nombre
SELECT COUNT(*) AS productos_sin_nombre
FROM Inventario
WHERE Nombre IS NULL OR Nombre = '';
-- Resultado esperado: 0

-- Productos sin unidad
SELECT COUNT(*) AS productos_sin_unidad
FROM Inventario
WHERE Unidad IS NULL OR Unidad = '';
-- Resultado esperado: 0

-- Tickets sin cajero asociado (relación huérfana)
SELECT t.Id, t.NumeroTicket
FROM Tickets t
LEFT JOIN Cajeros c ON t.CajeroId = c.Id
WHERE c.Id IS NULL;
-- Resultado esperado: 0 filas

-- Detalles de ticket sin ticket asociado (relación huérfana)
SELECT d.Id
FROM DetalleTicket d
LEFT JOIN Tickets t ON d.TicketId = t.Id
WHERE t.Id IS NULL;
-- Resultado esperado: 0 filas

-- ============================================================
-- 4. VALIDACIÓN DE RANGOS Y REGLAS DE NEGOCIO
-- ============================================================

-- Productos con cantidad negativa (error de negocio)
SELECT Nombre, Cantidad, Categoria
FROM Inventario
WHERE Cantidad < 0;
-- Resultado esperado: 0 filas

-- Tickets con total negativo
SELECT Id, NumeroTicket, Total
FROM Tickets
WHERE Total < 0;
-- Resultado esperado: 0 filas

-- Tickets con propina negativa
SELECT Id, NumeroTicket, Propina
FROM Tickets
WHERE Propina < 0;
-- Resultado esperado: 0 filas

-- Tickets donde el pago es menor que el total
SELECT Id, NumeroTicket, Total, Pago
FROM Tickets
WHERE Pago < Total AND Pago IS NOT NULL;
-- Resultado esperado: 0 filas (o revisar casos especiales)

-- Productos con stock crítico (cantidad = 0)
SELECT Nombre, Cantidad, Categoria
FROM Inventario
WHERE Cantidad = 0
ORDER BY Categoria, Nombre;

-- Productos con stock bajo (cantidad <= 5)
SELECT Nombre, Cantidad, Categoria
FROM Inventario
WHERE Cantidad <= 5 AND Cantidad > 0
ORDER BY Cantidad, Categoria;

-- ============================================================
-- 5. VALIDACIÓN DE INTEGRIDAD REFERENCIAL
-- ============================================================

-- Verificar que todos los tickets tienen detalles
SELECT t.Id, t.NumeroTicket
FROM Tickets t
LEFT JOIN DetalleTicket d ON t.Id = d.TicketId
WHERE d.Id IS NULL;
-- Tickets sin productos son inválidos

-- Verificar coherencia de totales (suma de detalles vs total del ticket)
SELECT 
    t.Id,
    t.NumeroTicket,
    t.Total AS total_registrado,
    COALESCE(SUM(d.Precio), 0) AS total_calculado,
    ABS(t.Total - COALESCE(SUM(d.Precio), 0)) AS diferencia
FROM Tickets t
LEFT JOIN DetalleTicket d ON t.Id = d.TicketId
GROUP BY t.Id, t.NumeroTicket, t.Total
HAVING ABS(t.Total - COALESCE(SUM(d.Precio), 0)) > 0.01;
-- Diferencia debe ser mínima (permitir 0.01 por redondeo)

-- ============================================================
-- 6. AUDITORÍA Y TRAZABILIDAD
-- ============================================================

-- Historial de cambios en inventario
SELECT COUNT(*) AS movimientos_inventario
FROM HistorialInventario;

-- Últimos 10 movimientos de inventario
SELECT 
    h.Id,
    i.Nombre AS producto,
    h.CantidadAnterior,
    h.CantidadNueva,
    h.Motivo,
    h.Fecha
FROM HistorialInventario h
INNER JOIN Inventario i ON h.ProductoId = i.Id
ORDER BY h.Fecha DESC
LIMIT 10;

-- Ventas por cajero
SELECT 
    c.Nombre AS cajero,
    COUNT(t.Id) AS total_tickets,
    SUM(t.Total) AS total_ventas,
    SUM(t.Propina) AS total_propinas
FROM Tickets t
INNER JOIN Cajeros c ON t.CajeroId = c.Id
GROUP BY c.Nombre
ORDER BY total_ventas DESC;

-- ============================================================
-- 7. ESTADÍSTICAS GENERALES
-- ============================================================

-- Resumen por tipo de orden
SELECT 
    TipoOrden,
    COUNT(*) AS cantidad_tickets,
    SUM(Total) AS total_ventas
FROM Tickets
GROUP BY TipoOrden;

-- Ticket promedio
SELECT 
    AVG(Total) AS ticket_promedio,
    MIN(Total) AS ticket_minimo,
    MAX(Total) AS ticket_maximo
FROM Tickets;

-- Productos más vendidos (top 10)
SELECT 
    d.Producto,
    COUNT(*) AS veces_vendido,
    SUM(d.Precio) AS ingresos_generados
FROM DetalleTicket d
WHERE d.EsExtra = 0
GROUP BY d.Producto
ORDER BY veces_vendido DESC
LIMIT 10;

-- ============================================================
-- FIN DE VALIDACIONES
-- ============================================================