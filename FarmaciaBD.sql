CREATE DATABASE FarmaciaDB;
GO

USE FarmaciaDB;
GO

-- crear usuario para conexi�n a la bd
-- Cambiar al contexto de la base de datos FarmaciaDB
USE FarmaciaDB;
GO

-- Crear el rol personalizado db_executor
CREATE ROLE db_executor;
GO

-- Otorgar permiso de ejecuci�n de procedimientos almacenados
GRANT EXECUTE TO db_executor;
GO
-- Crear un login en el servidor
CREATE LOGIN usr_far WITH PASSWORD = 'Segura123!';
GO

-- Crear el usuario dentro de la base de datos
CREATE USER usr_far FOR LOGIN usr_far;
GO

-- Asignar roles est�ndar (lectura, escritura, ejecuci�n de SPs)
EXEC sp_addrolemember 'db_datareader', 'usr_far';
EXEC sp_addrolemember 'db_datawriter', 'usr_far';
EXEC sp_addrolemember 'db_executor', 'usr_far'; -- solo si ya existe este rol personalizado
GO


-- Tabla de Usuarios
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario VARCHAR(50) NOT NULL,
    Contrase�a VARCHAR(255) NOT NULL,
    Rol VARCHAR(20) NOT NULL
);
GO

-- Tabla de Productos
CREATE TABLE Productos (
    ProductoID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion TEXT,
    Precio DECIMAL(10,2) NOT NULL,
    CantidadStock INT NOT NULL,
    StockMinimo INT NOT NULL
);
GO

-- Tabla de Ventas
CREATE TABLE Ventas (
    VentaID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NOT NULL,
    FechaVenta DATETIME NOT NULL DEFAULT GETDATE(),
    TotalVenta DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);
GO

-- Tabla de Detalles de Venta
CREATE TABLE DetalleVenta (
    DetalleID INT IDENTITY(1,1) PRIMARY KEY,
    VentaID INT NOT NULL,
    ProductoID INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (VentaID) REFERENCES Ventas(VentaID),
    FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);
GO

ALTER TABLE DetalleVenta
ADD PorcentajeDescuento DECIMAL(5,2) NULL;
GO

-- Tabla de Proveedores
CREATE TABLE Proveedores (
    ProveedorID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Contacto VARCHAR(100)
);
GO

CREATE TABLE ReglasPedido (
    ReglaID INT PRIMARY KEY IDENTITY(1,1),
    ProductoID INT NOT NULL,
    ProveedorID INT NOT NULL,
    CantidadSugerida INT NOT NULL,
    Activa BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID),
    FOREIGN KEY (ProveedorID) REFERENCES Proveedores(ProveedorID)
);


-- Tabla intermedia: ProveedorProducto
CREATE TABLE ProveedorProducto (
    ProveedorID INT NOT NULL,
    ProductoID INT NOT NULL,
    PRIMARY KEY (ProveedorID, ProductoID),
    FOREIGN KEY (ProveedorID) REFERENCES Proveedores(ProveedorID),
    FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);
GO


--Tabla Promociones
CREATE TABLE Promociones (
    PromocionID INT PRIMARY KEY IDENTITY(1,1),
    ProductoID INT NOT NULL,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    PorcentajeDescuento DECIMAL(5,2) NOT NULL,
    CONSTRAINT FK_Promociones_Productos FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);
GO

CREATE INDEX IX_Promociones_Producto_Fecha
ON Promociones (ProductoID, FechaInicio, FechaFin);
GO

CREATE TABLE Pedidos (
    PedidoID INT PRIMARY KEY IDENTITY(1,1),
    ProveedorID INT NOT NULL,
    FechaPedido DATE NOT NULL,
    FechaRecepcion DATE NULL,
    Estado VARCHAR(30) NOT NULL DEFAULT 'Pendiente', -- Pendiente, Parcialmente recibido, Recibido
    CONSTRAINT FK_Pedidos_Proveedor FOREIGN KEY (ProveedorID) REFERENCES Proveedores(ProveedorID)
);
GO

CREATE TABLE DetallePedido (
    DetallePedidoID INT PRIMARY KEY IDENTITY(1,1),
    PedidoID INT NOT NULL,
    ProductoID INT NOT NULL,
    CantidadSolicitada INT NOT NULL,
    CantidadRecibida INT NULL,
    CONSTRAINT FK_DetallePedido_Pedido FOREIGN KEY (PedidoID) REFERENCES Pedidos(PedidoID),
    CONSTRAINT FK_DetallePedido_Producto FOREIGN KEY (ProductoID) REFERENCES Productos(ProductoID)
);
GO

-- SP Insertar Usuario
CREATE PROCEDURE sp_insertarUsuario
	@NombreUsuario VARCHAR(50),
    @Contrase�a VARCHAR(255),
    @Rol VARCHAR(20)
AS
BEGIN
	INSERT INTO Usuarios (NombreUsuario, Contrase�a, Rol) 
	VALUES(@NombreUsuario, @Contrase�a, @Rol)
END
GO

exec sp_insertarUsuario 'superman','123456','Administrador'
GO

select * from Usuarios

-- SP Actualizar Usuario
CREATE PROCEDURE sp_actualizarUsuario
	@UsuarioID INT,
	@NombreUsuario VARCHAR(50),
    @Contrase�a VARCHAR(255),
    @Rol VARCHAR(20)
AS
BEGIN
	UPDATE Usuarios
	SET NombreUsuario = @NombreUsuario,
		Contrase�a = @Contrase�a,
		Rol = @Rol
	WHERE UsuarioID = @UsuarioID;
END
GO

-- SP Borrar Usuario
CREATE PROCEDURE sp_BorrarUsuario
	@UsuarioID INT
AS
BEGIN
 	DELETE Usuarios WHERE UsuarioID = @UsuarioID;
END
GO

CREATE PROCEDURE sp_ConsultarUsuario
    @UsuarioID INT = NULL
AS
BEGIN
    IF @UsuarioID IS NULL
    BEGIN
        SELECT UsuarioID, NombreUsuario, Contrase�a, Rol
        FROM Usuarios;
    END
    ELSE
    BEGIN
        SELECT UsuarioID, NombreUsuario, Contrase�a, Rol
        FROM Usuarios
        WHERE UsuarioID = @UsuarioID;
    END
END
GO



-- SP Insertar Producto
CREATE PROCEDURE sp_InsertarProducto
    @Nombre VARCHAR(100),
    @Descripcion TEXT,
    @Precio DECIMAL(10,2),
    @CantidadStock INT,
    @StockMinimo INT
AS
BEGIN
    INSERT INTO Productos (Nombre, Descripcion, Precio, CantidadStock, StockMinimo)
    VALUES (@Nombre, @Descripcion, @Precio, @CantidadStock, @StockMinimo);
END
GO

-- SP Actualizar Producto
CREATE PROCEDURE sp_ActualizarProducto
    @ProductoID INT,
    @Nombre VARCHAR(100),
    @Descripcion TEXT,
    @Precio DECIMAL(10,2),
    @CantidadStock INT,
    @StockMinimo INT
AS
BEGIN
    UPDATE Productos
    SET Nombre = @Nombre,
        Descripcion = @Descripcion,
        Precio = @Precio,
        CantidadStock = @CantidadStock,
        StockMinimo = @StockMinimo
    WHERE ProductoID = @ProductoID;
END
GO

-- SP Borrar Producto
CREATE PROCEDURE sp_EliminarProducto
    @ProductoID INT
AS
BEGIN
    DELETE FROM Productos WHERE ProductoID = @ProductoID;
END
GO

CREATE PROCEDURE SP_ConsultarProducto
    @ProductoID INT = NULL  -- si est� null devuelve todos los productos
AS
BEGIN
    SET NOCOUNT ON;

    IF @ProductoID IS NULL
    BEGIN
        SELECT * FROM Productos;
    END
    ELSE
    BEGIN
        SELECT * FROM Productos WHERE ProductoID = @ProductoID;
    END
END
GO

CREATE PROCEDURE sp_ObtenerProductoPorID
    @ProductoID INT
AS
BEGIN
    SELECT ProductoID, Nombre, Descripcion, Precio, CantidadStock, StockMinimo
    FROM Productos
    WHERE ProductoID = @ProductoID;
END
GO

CREATE PROCEDURE sp_ObtenerProductoPorNombre
    @Nombre NVARCHAR(100)
AS
BEGIN
    SELECT ProductoID, Nombre, Descripcion, Precio, CantidadStock, StockMinimo
    FROM Productos
    WHERE Nombre = @Nombre;
END
GO

CREATE PROCEDURE sp_ObtenerTodosProductos
AS
BEGIN
    SELECT ProductoID, Nombre, Descripcion, Precio, CantidadStock, StockMinimo
    FROM Productos;
END
GO


CREATE PROCEDURE sp_AsignarProveedorAProducto
    @ProductoID INT,
    @ProveedorID INT
AS
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM ProveedorProducto
        WHERE ProductoID = @ProductoID AND ProveedorID = @ProveedorID
    )
    BEGIN
        INSERT INTO ProveedorProducto (ProductoID, ProveedorID)
        VALUES (@ProductoID, @ProveedorID);
    END
END
GO


-- SP Insertar Proveedor
CREATE PROCEDURE sp_InsertarProveedor
    @Nombre VARCHAR(100),
    @Contacto VARCHAR(100)
AS
BEGIN
    INSERT INTO Proveedores (Nombre, Contacto)
    VALUES (@Nombre, @Contacto);
END
GO

-- SP Actualizar Proveedor
CREATE PROCEDURE sp_ActualizarProveedor
    @ProveedorID INT,
    @Nombre VARCHAR(100),
    @Contacto VARCHAR(100)
AS
BEGIN
    UPDATE Proveedores
    SET Nombre = @Nombre,
        Contacto = @Contacto
    WHERE ProveedorID = @ProveedorID;
END
GO

-- SP Borrar Proveedor
CREATE PROCEDURE sp_BorrarProveedor
    @ProveedorID INT
AS
BEGIN
    DELETE FROM Proveedores WHERE ProveedorID = @ProveedorID;
END
GO
-- SP Consultar Proveedores
CREATE PROCEDURE sp_ConsultarProveedores
    @ProveedorID INT = NULL
AS
BEGIN
    IF @ProveedorID IS NULL
        SELECT * FROM Proveedores;
    ELSE
        SELECT * FROM Proveedores WHERE ProveedorID = @ProveedorID;
END
GO
-- SP Consultar Proveedor por id
CREATE PROCEDURE sp_ConsultarProveedorPorId
	@ProveedorID INT
AS
BEGIN
    SELECT  ProveedorID, Nombre, Contacto FROM Proveedores
	WHERE ProveedorID = @ProveedorID;
END
GO

-- SP Consultar Todos los Proveedores
CREATE PROCEDURE sp_ConsultarProveedoresTodos
AS
BEGIN
    SELECT  ProveedorID, Nombre, Contacto FROM Proveedores;
END
GO

-- SP Insertar Ventas
CREATE PROCEDURE sp_InsertarVenta
    @UsuarioID INT,
    @FechaVenta DATETIME,
    @TotalVenta DECIMAL(18, 2),
    @VentaID INT OUTPUT
AS
BEGIN
    INSERT INTO Ventas (UsuarioID, FechaVenta, TotalVenta)
    VALUES (@UsuarioID, @FechaVenta, @TotalVenta);

    SET @VentaID = SCOPE_IDENTITY();
END
GO

-- SP Actualizar Ventas
CREATE PROCEDURE sp_ActualizarVenta
    @VentaID INT,
    @UsuarioID INT,
    @FechaVenta DATETIME,
    @TotalVenta DECIMAL(10,2)
AS
BEGIN
    UPDATE Ventas
    SET UsuarioID = @UsuarioID,
        FechaVenta = @FechaVenta,
        TotalVenta = @TotalVenta
    WHERE VentaID = @VentaID;
END
GO

-- SP Borrar Ventas
CREATE PROCEDURE sp_BorrarVenta
    @VentaID INT
AS
BEGIN
    DELETE FROM Ventas WHERE VentaID = @VentaID;
END
GO

-- SP Consultar Ventas por Id
CREATE PROCEDURE sp_ConsultarVentaPorId
@VentaID INT
AS
BEGIN
    SELECT * FROM Ventas WHERE VentaID = @VentaID;
END
GO

-- SP Consultar todas las Ventas 
CREATE PROCEDURE sp_ConsultarVentasTodas
AS
BEGIN
    SELECT  VentaID, UsuarioID, FechaVenta, TotalVenta  FROM Ventas;
END
GO

--SP Consultar VentaId con los dem�s datos
CREATE PROCEDURE sp_ConsultarVentaID
 
    @UsuarioID INT,
    @FechaVenta DATETIME,
    @TotalVenta DECIMAL(10,2)
AS
BEGIN
	SELECT VentaID FROM Ventas 
	WHERE UsuarioID = @UsuarioID AND FechaVenta = @FechaVenta AND TotalVenta = @TotalVenta;
END
GO

CREATE PROCEDURE SP_ConsultarVenta
    @VentaID INT
AS
BEGIN
    SELECT VentaID, UsuarioID, FechaVenta, TotalVenta
    FROM Ventas
    WHERE VentaID = @VentaID
END
GO

-- SP Insertar DetalleVenta
CREATE OR ALTER PROCEDURE SP_InsertarDetalleVenta
    @VentaID INT,
    @ProductoID INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(18,2),
    @PorcentajeDescuento DECIMAL(5,2)
AS
BEGIN
    INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario, PorcentajeDescuento)
    VALUES (@VentaID, @ProductoID, @Cantidad, @PrecioUnitario, @PorcentajeDescuento)
END
GO

--SP ActualizarStockProducto

CREATE PROCEDURE sp_ActualizarStockProducto
    @ProductoID INT,
    @CantidadVendida INT,
    @NuevoStock INT OUTPUT
AS
BEGIN
    UPDATE Productos
    SET CantidadStock = CantidadStock - @CantidadVendida
    WHERE ProductoID = @ProductoID;

    SELECT @NuevoStock = CantidadStock FROM Productos WHERE ProductoID = @ProductoID;
END
GO



-- SP Actualizar DetalleVenta
CREATE PROCEDURE sp_ActualizarDetalleVenta
    @DetalleID INT,
    @VentaID INT,
    @ProductoID INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2)
AS
BEGIN
    UPDATE DetalleVenta
    SET VentaID = @VentaID,
        ProductoID = @ProductoID,
        Cantidad = @Cantidad,
        PrecioUnitario = @PrecioUnitario
    WHERE DetalleID = @DetalleID;
END
GO
-- SP Borrar DetalleVenta
CREATE PROCEDURE sp_BorrarDetalleVenta
    @DetalleID INT
AS
BEGIN
    DELETE FROM DetalleVenta WHERE DetalleID = @DetalleID;
END
GO

-- SP Consultar DetalleVenta
CREATE OR ALTER PROCEDURE SP_ConsultarDetalleVenta
    @VentaID INT
AS
BEGIN
    SELECT 
        dv.ProductoID,
        p.Nombre AS NombreProducto,
        dv.Cantidad,
        dv.PrecioUnitario,
        dv.PorcentajeDescuento
    FROM DetalleVenta dv
    INNER JOIN Productos p ON dv.ProductoID = p.ProductoID
    WHERE dv.VentaID = @VentaID
END
GO


select * from DetalleVenta


-- SP Consultar DetalleVenta Por VentaID
CREATE PROCEDURE sp_ConsultarDetalleVentaPorVentaID
@VentaID INT
AS
BEGIN
    SELECT * FROM DetalleVenta WHERE VentaID = @VentaID;
END
GO

-- Insertar relaci�n Proveedor - Producto
CREATE PROCEDURE sp_InsertarProveedorProducto
    @ProveedorID INT,
    @ProductoID INT
AS
BEGIN
    INSERT INTO ProveedorProducto (ProveedorID, ProductoID)
    VALUES (@ProveedorID, @ProductoID);
END
GO

-- Borrar relaci�n Proveedor - Producto
CREATE PROCEDURE sp_BorrarProveedorProducto
    @ProveedorID INT,
    @ProductoID INT
AS
BEGIN
    DELETE FROM ProveedorProducto
    WHERE ProveedorID = @ProveedorID AND ProductoID = @ProductoID;
END
GO

-- Consultar relaciones Proveedor - Producto
CREATE PROCEDURE sp_ConsultarProveedorProducto
AS
BEGIN
    SELECT 
        pp.ProveedorID,
        p.Nombre AS NombreProveedor,
        pp.ProductoID,
        pr.Nombre AS NombreProducto
    FROM ProveedorProducto pp
    INNER JOIN Proveedores p ON pp.ProveedorID = p.ProveedorID
    INNER JOIN Productos pr ON pp.ProductoID = pr.ProductoID;
END
GO


--SP_ActualizarStock

CREATE PROCEDURE sp_ActualizarStock
    @ProductoID INT,
    @CantidadVendida INT
AS
BEGIN
    DECLARE @StockActual INT;

    SELECT @StockActual = CantidadStock FROM Productos WHERE ProductoID = @ProductoID;

    IF @StockActual >= @CantidadVendida
    BEGIN
        UPDATE Productos
        SET CantidadStock = CantidadStock - @CantidadVendida
        WHERE ProductoID = @ProductoID;
    END
    ELSE
    BEGIN
        RAISERROR('Stock insuficiente para el producto.', 16, 1);
    END
END
GO


--SP_GenerarAlertaStockMinimo
CREATE PROCEDURE sp_GenerarAlertaStockMinimo
AS
BEGIN
    SELECT ProductoID, Nombre, CantidadStock, StockMinimo
    FROM Productos
    WHERE CantidadStock < StockMinimo;
END
GO

--SP_CalcularDescuento -- Validar bien como es el funcionamiento del descuento para ver si es necesario crearle una tabla...




--SP_InsertarVenta

CREATE TYPE DetalleVentaTipo AS TABLE (
    ProductoID INT,
    Cantidad INT,
    PrecioUnitario DECIMAL(10,2)
);
GO

CREATE PROCEDURE sp_InsertarVentas
    @UsuarioID INT,
    @FechaVenta DATETIME,
    @DetalleVenta DetalleVentaTipo READONLY
AS
BEGIN
    DECLARE @TotalVenta DECIMAL(10,2) = 0;
    DECLARE @VentaID INT;

    BEGIN TRANSACTION;

    -- Calcular total
    SELECT @TotalVenta = SUM(Cantidad * PrecioUnitario)
    FROM @DetalleVenta;

    -- Insertar encabezado de venta
    INSERT INTO Ventas (UsuarioID, FechaVenta, TotalVenta)
    VALUES (@UsuarioID, @FechaVenta, @TotalVenta);

    SET @VentaID = SCOPE_IDENTITY();

    -- Insertar detalles y actualizar stock
    DECLARE @ProductoID INT, @Cantidad INT, @Precio DECIMAL(10,2);

    DECLARE detalle_cursor CURSOR FOR
    SELECT ProductoID, Cantidad, PrecioUnitario FROM @DetalleVenta;

    OPEN detalle_cursor;
    FETCH NEXT FROM detalle_cursor INTO @ProductoID, @Cantidad, @Precio;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Insertar detalle
        INSERT INTO DetalleVenta (VentaID, ProductoID, Cantidad, PrecioUnitario)
        VALUES (@VentaID, @ProductoID, @Cantidad, @Precio);

        -- Actualizar stock
        EXEC sp_ActualizarStock @ProductoID, @Cantidad;

        FETCH NEXT FROM detalle_cursor INTO @ProductoID, @Cantidad, @Precio;
    END

    CLOSE detalle_cursor;
    DEALLOCATE detalle_cursor;

    COMMIT TRANSACTION;
END
GO



--SP_ObtenerReporteVentas
CREATE PROCEDURE [dbo].[sp_ObtenerReporteVentas]
    @FechaInicio DATETIME,
    @FechaFin DATETIME
AS
BEGIN
    SELECT 
        v.VentaID,
        v.FechaVenta,
        u.NombreUsuario,
        v.TotalVenta,
        dv.ProductoID,
        p.Nombre AS NombreProducto,
        dv.Cantidad,
        dv.PrecioUnitario,
		dv.PorcentajeDescuento,
		(dv.PrecioUnitario * dv.PorcentajeDescuento)/100 as ImporteDescuento,
		dv.Cantidad * (dv.PrecioUnitario - (dv.PrecioUnitario * dv.PorcentajeDescuento)/100) as TotalProducto
    FROM Ventas v
    INNER JOIN Usuarios u ON v.UsuarioID = u.UsuarioID
    INNER JOIN DetalleVenta dv ON v.VentaID = dv.VentaID
    INNER JOIN Productos p ON dv.ProductoID = p.ProductoID
    WHERE v.FechaVenta BETWEEN @FechaInicio AND @FechaFin
    ORDER BY v.FechaVenta;
END
GO




CREATE PROCEDURE sp_BuscarProveedorPorNombre
    @Nombre VARCHAR(100)
AS
BEGIN
    SELECT * FROM Proveedores
    WHERE Nombre LIKE '%' + @Nombre + '%'
END
GO

CREATE PROCEDURE sp_ObtenerUsuarioPorNombre
    @NombreUsuario VARCHAR(50)
AS
BEGIN
    SELECT UsuarioID, NombreUsuario, Contrase�a, Rol
    FROM Usuarios
    WHERE NombreUsuario = @NombreUsuario;
END
GO

CREATE PROCEDURE sp_EliminarProveedorDeProducto
    @ProductoID INT,
    @ProveedorID INT
AS
BEGIN
    DELETE FROM ProveedorProducto
    WHERE ProductoID = @ProductoID AND ProveedorID = @ProveedorID;
END
GO

CREATE PROCEDURE sp_ObtenerProveedoresPorProducto
    @ProductoID INT
AS
BEGIN
    SELECT ProveedorID
    FROM ProveedorProducto
    WHERE ProductoID = @ProductoID;
END
GO

CREATE PROCEDURE sp_ObtenerProductosPorProveedor
    @ProveedorID INT
AS
BEGIN
    SELECT ProductoID
    FROM ProveedorProducto
    WHERE ProveedorID = @ProveedorID;
END
GO

-- SP_InsertarPromocion
CREATE PROCEDURE SP_InsertarPromocion
    @ProductoID INT,
    @FechaInicio DATE,
    @FechaFin DATE,
    @PorcentajeDescuento DECIMAL(5,2)
AS
BEGIN
    INSERT INTO Promociones (ProductoID, FechaInicio, FechaFin, PorcentajeDescuento)
    VALUES (@ProductoID, @FechaInicio, @FechaFin, @PorcentajeDescuento)
END
GO

-- SP_ActualizarPromocion
CREATE PROCEDURE SP_ActualizarPromocion
    @PromocionID INT,
    @ProductoID INT,
    @FechaInicio DATE,
    @FechaFin DATE,
    @PorcentajeDescuento DECIMAL(5,2)
AS
BEGIN
    UPDATE Promociones
    SET ProductoID = @ProductoID,
        FechaInicio = @FechaInicio,
        FechaFin = @FechaFin,
        PorcentajeDescuento = @PorcentajeDescuento
    WHERE PromocionID = @PromocionID
END
GO

-- SP_EliminarPromocion
CREATE PROCEDURE SP_EliminarPromocion
    @PromocionID INT
AS
BEGIN
    DELETE FROM Promociones WHERE PromocionID = @PromocionID
END
GO

-- SP_ObtenerTodasPromociones
CREATE PROCEDURE SP_ObtenerTodasPromociones
AS
BEGIN
    SELECT PromocionID, ProductoID, FechaInicio, FechaFin, PorcentajeDescuento
    FROM Promociones
END
GO

-- SP_ObtenerPromocionVigentePorProducto
CREATE PROCEDURE SP_ObtenerPromocionVigentePorProducto
    @ProductoID INT,
    @Fecha DATE
AS
BEGIN
    SELECT TOP 1 PromocionID, ProductoID, FechaInicio, FechaFin, PorcentajeDescuento
    FROM Promociones
    WHERE ProductoID = @ProductoID
      AND @Fecha BETWEEN FechaInicio AND FechaFin
    ORDER BY FechaInicio DESC
END
GO

CREATE OR ALTER PROCEDURE SP_CrearPedido
    @ProveedorID INT,
    @FechaPedido DATE,
    @PedidoID INT OUTPUT
AS
BEGIN
    INSERT INTO Pedidos (ProveedorID, FechaPedido, Estado)
    VALUES (@ProveedorID, @FechaPedido, 'Pendiente');

    SET @PedidoID = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE SP_InsertarDetallePedido
    @PedidoID INT,
    @ProductoID INT,
    @CantidadSolicitada INT
AS
BEGIN
    INSERT INTO DetallePedido (PedidoID, ProductoID, CantidadSolicitada)
    VALUES (@PedidoID, @ProductoID, @CantidadSolicitada);
END
GO

CREATE OR ALTER PROCEDURE SP_ObtenerPedidosPendientes
AS
BEGIN
    SELECT p.PedidoID, p.FechaPedido, p.Estado, pr.Nombre AS ProveedorNombre
    FROM Pedidos p
    JOIN Proveedores pr ON p.ProveedorID = pr.ProveedorID
    WHERE p.Estado IN ('Pendiente', 'Parcialmente recibido');
END
GO

CREATE OR ALTER PROCEDURE SP_ObtenerDetallePedido
    @PedidoID INT
AS
BEGIN
    SELECT dp.DetallePedidoID, dp.ProductoID, pr.Nombre AS NombreProducto,
           dp.CantidadSolicitada, dp.CantidadRecibida
    FROM DetallePedido dp
    JOIN Productos pr ON dp.ProductoID = pr.ProductoID
    WHERE dp.PedidoID = @PedidoID;
END
GO

CREATE OR ALTER PROCEDURE SP_RegistrarRecepcionPedido
    @PedidoID INT
AS
BEGIN
    DECLARE @Total INT, @Recibidos INT;

    -- Actualiza la fecha de recepci�n
    UPDATE Pedidos
    SET FechaRecepcion = GETDATE()
    WHERE PedidoID = @PedidoID;

    -- Verifica estado del pedido
    SELECT @Total = COUNT(*)
    FROM DetallePedido
    WHERE PedidoID = @PedidoID;

    SELECT @Recibidos = COUNT(*)
    FROM DetallePedido
    WHERE PedidoID = @PedidoID AND CantidadRecibida = CantidadSolicitada;

    IF @Recibidos = @Total
        UPDATE Pedidos SET Estado = 'Recibido' WHERE PedidoID = @PedidoID;
    ELSE
        UPDATE Pedidos SET Estado = 'Parcialmente recibido' WHERE PedidoID = @PedidoID;
END
GO

CREATE OR ALTER PROCEDURE SP_ActualizarCantidadRecibida
    @DetallePedidoID INT,
    @CantidadRecibida INT
AS
BEGIN
    UPDATE DetallePedido
    SET CantidadRecibida = @CantidadRecibida
    WHERE DetallePedidoID = @DetallePedidoID;
END
GO

-- Cambia estado del pedido y fecha de recepci�n
CREATE PROCEDURE SP_RegistrarRecepcionPedido
    @PedidoID INT
AS
BEGIN
    UPDATE Pedidos
    SET Estado = 'Recibido',
        FechaRecepcion = GETDATE()
    WHERE PedidoID = @PedidoID
END
GO

-- Registra la cantidad recibida por detalle
CREATE PROCEDURE SP_RegistrarRecepcionDetalle
    @DetallePedidoID INT,
    @CantidadRecibida INT
AS
BEGIN
    UPDATE DetallePedidos
    SET CantidadRecibida = @CantidadRecibida
    WHERE DetallePedidoID = @DetallePedidoID

    -- Tambi�n actualiza el inventario
    UPDATE Productos
    SET CantidadStock = CantidadStock + @CantidadRecibida
    WHERE ProductoID = (SELECT ProductoID FROM DetallePedidos WHERE DetallePedidoID = @DetallePedidoID)
END
GO

CREATE PROCEDURE SP_AumentarStockProducto
    @ProductoID INT,
    @Cantidad INT
AS
BEGIN
    UPDATE Productos
    SET CantidadStock = CantidadStock + @Cantidad
    WHERE ProductoID = @ProductoID;
END
GO

CREATE PROCEDURE SP_ActualizarEstadoPedido
    @PedidoID INT,
    @FechaRecepcion DATETIME,
    @Estado NVARCHAR(50)
AS
BEGIN
    UPDATE Pedidos
    SET FechaRecepcion = @FechaRecepcion,
        Estado = @Estado
    WHERE PedidoID = @PedidoID;
END
GO

CREATE PROCEDURE SP_InsertarReglaPedido
    @ProductoID INT,
    @ProveedorID INT,
    @CantidadSugerida INT,
    @Activa BIT
AS
BEGIN
    INSERT INTO ReglasPedido (ProductoID, ProveedorID, CantidadSugerida, Activa)
    VALUES (@ProductoID, @ProveedorID, @CantidadSugerida, @Activa);
END
GO

CREATE PROCEDURE SP_ActualizarReglaPedido
    @ReglaID INT,
    @ProductoID INT,
    @ProveedorID INT,
    @CantidadSugerida INT,
    @Activa BIT
AS
BEGIN
    UPDATE ReglasPedido
    SET ProductoID = @ProductoID,
        ProveedorID = @ProveedorID,
        CantidadSugerida = @CantidadSugerida,
        Activa = @Activa
    WHERE ReglaID = @ReglaID;
END
GO

CREATE PROCEDURE SP_EliminarReglaPedido
    @ReglaID INT
AS
BEGIN
    DELETE FROM ReglasPedido
    WHERE ReglaID = @ReglaID;
END
GO

CREATE PROCEDURE SP_ObtenerReglaPedidoPorProducto
    @ProductoID INT
AS
BEGIN
    SELECT TOP 1 *
    FROM ReglasPedido
    WHERE ProductoID = @ProductoID AND Activa = 1;
END
GO

CREATE PROCEDURE SP_ObtenerTodasReglasPedido
AS
BEGIN
    SELECT *
    FROM ReglasPedido;
END
GO

CREATE PROCEDURE SP_ObtenerReglaPedidoPorProducto
    @ProductoID INT
AS
BEGIN
    SELECT TOP 1
        ReglaID,
        ProductoID,
        ProveedorID,
        CantidadSugerida,
        Activa
    FROM ReglasPedido
    WHERE ProductoID = @ProductoID AND Activa = 1
    ORDER BY ReglaID DESC
END
GO

CREATE PROCEDURE SP_InsertarPedidoAutomatico
    @ProductoID INT,
    @ProveedorID INT,
    @Cantidad INT
AS
BEGIN
    DECLARE @PedidoID INT;

    -- Insertar pedido
    INSERT INTO Pedidos (ProveedorID, FechaPedido, Estado)
    VALUES (@ProveedorID, GETDATE(), 'Pendiente');

    SET @PedidoID = SCOPE_IDENTITY();

    -- Insertar detalle del pedido
    INSERT INTO DetallePedido (PedidoID, ProductoID, CantidadSolicitada)
    VALUES (@PedidoID, @ProductoID, @Cantidad);
END
GO

