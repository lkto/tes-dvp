CREATE DATABASE LabDev;

USE LabDev;

-- Tables

CREATE TABLE CatTipoCliente
(
	Id INT IDENTITY (1, 1) PRIMARY KEY,
	TipoCliente VARCHAR(50) NOT NULL
);

CREATE TABLE TblClientes
(
	Id INT IDENTITY (1, 1) PRIMARY KEY,
	RazonSocial VARCHAR(200) NOT NULL,
	IdTipoCliente INT REFERENCES CatTipoCliente(Id),
	FechaCreacion DATETIME DEFAULT GETDATE(),
	RFC VARCHAR(50) NOT NULL
);

CREATE TABLE CatProductos
(
	Id INT IDENTITY (1, 1) PRIMARY KEY,
	NombreProducto VARCHAR(50) NOT NULL,
	ImagenProducto VARCHAR(250),
	PrecioUnitario DECIMAL(18, 2) NOT NULL,
	Ext VARCHAR(5)
);

CREATE TABLE TblFacturas
(
	Id INT IDENTITY (1, 1) PRIMARY KEY,
	FechaEmisionFactura DATETIME DEFAULT GETDATE(),
	IdCliente INT REFERENCES TblClientes(Id),
	NumeroFactura INT NOT NULL,
	NumeroTotalArticulos INT NOT NULL,
	SubTotalFactura DECIMAL(18, 2) NOT NULL,
	TotalImpuesto DECIMAL(18, 2) NOT NULL,
	TotalFactura DECIMAL(18, 2) NOT NULL,
);

CREATE TABLE TblDetallesFactura
(
	Id INT IDENTITY (1, 1) PRIMARY KEY,
	IdFactura INT REFERENCES TblFacturas(Id),
	IdProducto INT REFERENCES CatProductos(Id),
	CantidadDeProducto INT NOT NULL,
	PrecioUnitarioProducto DECIMAL(18, 2) NOT NULL,
	SubtotalProducto DECIMAL(18, 2) NOT NULL,
	Notas VARCHAR(200)
);

-- Stored Procedures

GO

CREATE PROCEDURE sp_ListarFacturas
AS
BEGIN
	SELECT Id, NumeroFactura,
		CONVERT(char(19), FechaEmisionFactura, 20) AS 'FechaEmision',
		TotalFactura
	FROM TblFacturas
END;

GO

CREATE PROCEDURE sp_BuscarFactura(@IdFactura INT)
AS
BEGIN
	SELECT Id, CONVERT(char(19), FechaEmisionFactura, 20) AS 'FechaEmision',
		IdCliente, NumeroFactura, NumeroTotalArticulos, SubTotalFactura,
		TotalImpuesto, TotalFactura
	FROM TblFacturas
	WHERE Id = @IdFactura
END;

GO

CREATE PROCEDURE sp_BuscarFacturaPorCliente(@IdCliente INT)
AS
BEGIN
	SELECT Id, NumeroFactura,
		CONVERT(char(19), FechaEmisionFactura, 20) AS 'FechaEmision',
		TotalFactura
	FROM TblFacturas
	WHERE IdCliente = @IdCliente
END;

GO

CREATE PROCEDURE sp_BuscarFacturaPorNumero(@NumeroFactura INT)
AS
BEGIN
	SELECT Id, NumeroFactura,
		CONVERT(char(19), FechaEmisionFactura, 20) AS 'FechaEmision',
		TotalFactura
	FROM TblFacturas
	WHERE NumeroFactura = @NumeroFactura
END;

GO

CREATE PROCEDURE sp_BuscarDetalleFactura(@IdFactura INT)
AS
BEGIN
	SELECT detalle.Id, IdFactura, IdProducto, producto.NombreProducto, producto.ImagenProducto,
		CantidadDeProducto, PrecioUnitarioProducto, SubTotalProducto, Notas
	FROM TblDetallesFactura AS detalle
		INNER JOIN CatProductos AS producto ON detalle.IdProducto = producto.Id
	WHERE detalle.Id = @IdFactura
END;

GO

CREATE PROCEDURE sp_InsertarFactura(
	@IdCliente INT,
	@NumeroFactura INT,
	@NumeroTotalArticulos INT,
	@SubTotalFactura DECIMAL(18, 2),
	@TotalImpuesto DECIMAL(18, 2),
	@TotalFactura DECIMAL(18, 2)
)
AS
BEGIN
	INSERT INTO TblFacturas
		(IdCliente, NumeroFactura, NumeroTotalArticulos,
		SubTotalFactura, TotalImpuesto, TotalFactura)
	VALUES(@IdCliente, @NumeroFactura, @NumeroTotalArticulos, @SubTotalFactura,
			@TotalImpuesto, @TotalFactura);

	SELECT CAST(SCOPE_IDENTITY() AS INT)
END;

GO

CREATE PROCEDURE sp_InsertarDetalleFactura(
	@IdFactura INT,
	@IdProducto INT,
	@CantidadDeProducto INT,
	@PrecioUnitarioProducto DECIMAL(18, 2),
	@SubTotalProducto DECIMAL(18, 2),
	@Notas VARCHAR(200)
)
AS
BEGIN
	INSERT INTO TblDetallesFactura
		(IdFactura, IdProducto, CantidadDeProducto, PrecioUnitarioProducto,
		SubtotalProducto, Notas)
	VALUES(@IdFactura, @IdProducto, @CantidadDeProducto, @PrecioUnitarioProducto, @SubTotalProducto,
			@Notas);

	SELECT CAST(SCOPE_IDENTITY() AS INT)
END;

GO

CREATE PROCEDURE sp_ListarClientes
AS
BEGIN
	SELECT cliente.Id, RazonSocial, RFC, tipo.Id AS 'IdTipoCliente',
		tipo.TipoCliente, CONVERT(char(19), FechaCreacion, 20) AS 'FechaCreacion'
	FROM TblClientes AS cliente
		INNER JOIN CatTipoCliente AS tipo ON cliente.IdTipoCliente = tipo.Id
END;

GO

CREATE PROCEDURE sp_ListarProductos
AS
BEGIN
	SELECT Id, NombreProducto, ImagenProducto, PrecioUnitario, Ext
	FROM CatProductos
END;

GO

CREATE PROCEDURE sp_BuscarProductoPorId(@Id int)
AS
BEGIN
	SELECT Id, NombreProducto, ImagenProducto, PrecioUnitario, Ext
	FROM CatProductos
	WHERE Id = @Id
END;

GO

-- Seed data

INSERT INTO CatProductos
	(NombreProducto, ImagenProducto, PrecioUnitario, Ext)
VALUES
	('Laptop', 'https://pactechmayoreo.com.mx/web/image/product.template/5463/image_256', 899.99, ''),
	('Telefono Inteligente', 'https://www.palser.com/imgs/productos/productos31_27652.jpg', 599.99, ''),
	('Auriculares Inalambricos', 'https://www.nnet.com.uy/productos/imgs/auriculares-inalambricos-energy-sistem-style-4-stone-bluetooth-128684-31.jpg', 129.99, ''),
	('Smartwatch', 'https://www.digitaloutlet.com.uy/imgs/productos/productos31_11530.jpg', 199.99, ''),
	('Altavoces Bluetooth', 'https://sampler.com.uy/imgs/productos/productos31_77346.jpg', 79.99, ''),
	('Camara de Seguridad', 'https://www.computerssalejalapa.com/web/image/product.template/3182/image_256/%5B303102412%5D%20%E2%80%8BC%C3%A1mara%20de%20Seguridad%20Ezviz%20EZC6N1C2?unique=b83a359', 149.99, ''),
	('Tableta', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQVKUBQ4enQJB8Gi0N2pjXFlSddABLwMYIxUaCUKJ9PGqozxLhY8RHafeh8pX5dmZPJzaQ&usqp=CAU', 349.99, ''),
	('Teclado mecanico', 'https://www.okcomputers-uy.com/imgs/productos/productos31_19466.jpg', 89.99, ''),
	('Mouse Gaming', 'https://computerssale.odoo.com/web/image/product.template/4759/image_256/%5BXTM-720%5D%20Mouse%20Gaming%20Xtech%20Combative%20XTM-720?unique=4c43b38', 49.99, ''),
	('Cargador Inalambrico', 'https://www.digitaloutlet.com.uy/imgs/productos/productos31_14813.jpg', 29.99, '');

INSERT INTO CatTipoCliente
	(TipoCliente)
VALUES
	('Cliente Regular'),
	('Cliente VIP'),
	('Cliente Corporativo'),
	('Cliente Nuevo'),
	('Cliente Leal');

INSERT INTO TblClientes
	(RazonSocial, IdTipoCliente, RFC)
VALUES
	('ABC Corporation', 1, 'RFC123456A'),
	('XYZ Enterprises', 2, 'RFC789012B'),
	('LMN Inc.', 3, 'RFC345678C'),
	('PQR Industries', 1, 'RFC901234D'),
	('123 Company', 2, 'RFC567890E'),
	('Global Solutions', 3, 'RFC123456F'),
	('Tech Innovators', 1, 'RFC789012G'),
	('Smart Ventures', 2, 'RFC345678H'),
	('Data Experts', 3, 'RFC901234I'),
	('Future Enterprises', 1, 'RFC567890J');
