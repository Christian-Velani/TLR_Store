CREATE DATABASE BD_TLRStore
GO

USE BD_TLRStore
GO

-- Criando as Tabelas
CREATE TABLE JOGOS
(
	idJogo							INT				NOT NULL	PRIMARY KEY IDENTITY,
	nome							VARCHAR(255)	NOT NULL	UNIQUE,
	imagem							VARBINARY(MAX)	NOT NULL,
	descricao						VARCHAR(MAX)	NOT NULL,
	preco							DECIMAL(10,2)	NOT NULL,
	dataLancamento					DATE			NOT NULL,
	classificacaoIndicativa			INT				NOT NULL	CHECK(classificacaoIndicativa in (3, 7, 12, 16, 18)),
	requisitos						VARCHAR(MAX)	NULL,
	status							INT				NOT NULL	CHECK(status in (0,1))
)
GO

CREATE TABLE GENEROS
(
	idGenero	INT				NOT NULL	PRIMARY KEY		IDENTITY,
	nome		VARCHAR(30)		NOT NULL	UNIQUE
)
GO
	

CREATE TABLE JOGOS_GENEROS
(
	jogoId		INT		NOT NULL,
	generoId	INT		NOT NULL
)
GO

CREATE TABLE TIPOS
(
	idTipo		INT				NOT NULL	PRIMARY KEY		IDENTITY,
	nome		VARCHAR(50)		NOT NULL	UNIQUE
)
GO

CREATE TABLE JOGOS_TIPOS
(
	jogoId	INT		NOT NULL,
	tipoId	INT		NOT NULL
)
GO

CREATE TABLE EMPRESAS
(
	idEmpresa	INT			NOT NULL	PRIMARY KEY		IDENTITY,
	nome		VARCHAR(50)	NOT NULL	UNIQUE
)
GO

CREATE TABLE JOGOS_EMPRESAS
(
	jogoId			INT		NOT NULL,
	empresaId		INT		NOT NULL,
	tipoEmpresa		INT		NOT NULL	CHECK (tipoEmpresa in (1, 2))
)
GO

CREATE TABLE USUARIOS
(
	idUsuario	INT						NOT NULL	PRIMARY KEY IDENTITY,
	icone		VARBINARY(MAX)			NULL,
	nome		VARCHAR(MAX)			NOT NULL,
	nick		VARCHAR(15)				NOT NULL		UNIQUE,
	senha		VARCHAR(20)				NOT NULL,
	email		VARCHAR(255)			NOT NULL		UNIQUE,
	status		INT						NOT NULL 		CHECK(status in (0,1)),
	tipo        INT						NOT NULL	CHECK(tipo in (0,1))
)
GO

CREATE TABLE JOGOS_USUARIOS
(
	jogoId		INT		NOT NULL,
	usuarioId	INT		NOT NULL
)
GO

CREATE TABLE COMPLEMENTOS
(
	idComplemento		INT					NOT NULL	PRIMARY KEY IDENTITY,
	nome				VARCHAR(255)		NOT NULL	UNIQUE,
	imagem				VARBINARY(MAX)		NOT NULL,
	preco				DECIMAL(10,2)		NOT NULL,
	descricao			VARCHAR(MAX)		NOT NULL,
	jogoId				INT					NOT NULL,
	status				INT					NOT NULL	CHECK(status in (0,1))
)
GO

CREATE TABLE JOGOS_COMPLEMENTOS
(
	complementoId INT		NOT NULL,
	jogoId		  INT		NOT NULL
)
GO

CREATE TABLE COMPLEMENTOS_USUARIOS
(
	complementoId		INT		NOT NULL,
	usuarioId			INT		NOT NULL
)
GO

CREATE TABLE PEDIDOS
(
	idPedido			INT				NOT NULL			PRIMARY KEY IDENTITY,
	meioPagamento		INT				NOT NULL			CHECK(meioPagamento in (1,2,3)),
	dataCompra			DATE			NOT NULL,
	valorCompra			DECIMAL(10,2)	NULL,
	usuarioId			INT				NOT NULL
)
GO

CREATE TABLE PRODUTOS_PEDIDOS
(
	pedidoId		INT			NOT NULL,
	jogoId			INT			NULL,
	complementoId	INT			NULL,
	idProdutoPedido	INT			NOT NULL		PRIMARY KEY	IDENTITY
)
GO

-- Adicionando as keys
ALTER TABLE JOGOS_GENEROS
ADD CONSTRAINT FK_JOGOSGENEROS_JOGOS
FOREIGN KEY (jogoId)
REFERENCES JOGOS(idJogo)
GO

ALTER TABLE JOGOS_GENEROS
ADD CONSTRAINT FK_JOGOSGENEROS_GENEROS
FOREIGN KEY (generoId)
REFERENCES GENEROS(idGenero)
GO

ALTER TABLE JOGOS_GENEROS
ADD CONSTRAINT PK_JOGOSGENEROS
PRIMARY KEY (jogoId, generoId)
GO

ALTER TABLE JOGOS_TIPOS
ADD CONSTRAINT FK_JOGOSTIPOS_JOGOS
FOREIGN KEY (jogoId)
REFERENCES JOGOS(idJogo)
GO

ALTER TABLE JOGOS_TIPOS
ADD CONSTRAINT FK_JOGOSTIPOS_TIPOS
FOREIGN KEY (tipoId)
REFERENCES TIPOS(idTipo)
GO

ALTER TABLE JOGOS_TIPOS
ADD CONSTRAINT PK_JOGOSTIPOS
PRIMARY KEY (jogoId, tipoId)
GO

ALTER TABLE JOGOS_EMPRESAS
ADD CONSTRAINT FK_JOGOSEMPRESAS_JOGOS
FOREIGN KEY (jogoId)
REFERENCES JOGOS(idJogo)
GO

ALTER TABLE JOGOS_EMPRESAS
ADD CONSTRAINT FK_JOGOSEMPRESAS_EMPRESAS
FOREIGN KEY (empresaId)
REFERENCES EMPRESAS(idEmpresa)
GO

ALTER TABLE JOGOS_USUARIOS
ADD CONSTRAINT FK_JOGOSUSUARIOS_JOGOS
FOREIGN KEY (jogoId)
REFERENCES JOGOS(idJogo)
GO

ALTER TABLE JOGOS_USUARIOS
ADD CONSTRAINT FK_JOGOSUSUARIOS_USUARIOS
FOREIGN KEY (usuarioId)
REFERENCES USUARIOS(idUsuario)
GO

ALTER TABLE JOGOS_USUARIOS
ADD CONSTRAINT PK_JOGOSUSUARIOS
PRIMARY KEY (jogoId, usuarioId)
GO

ALTER TABLE COMPLEMENTOS
ADD CONSTRAINT FK_COMPLEMENTOS_JOGOS
FOREIGN KEY (jogoId)
REFERENCES JOGOS(idJogo)
GO

ALTER TABLE COMPLEMENTOS_USUARIOS
ADD CONSTRAINT FK_COMPLEMENTOSUSUARIOS_COMPLEMENTOS
FOREIGN KEY (complementoId)
REFERENCES COMPLEMENTOS(idComplemento)
GO

ALTER TABLE COMPLEMENTOS_USUARIOS
ADD CONSTRAINT FK_COMPLEMENTOSUSUARIOS_USUARIOS
FOREIGN KEY (usuarioId)
REFERENCES USUARIOS(idUsuario)
GO

ALTER TABLE COMPLEMENTOS_USUARIOS
ADD CONSTRAINT PK_COMPLEMENTOSUSUARIOS
PRIMARY KEY (complementoId, usuarioId)
GO

ALTER TABLE JOGOS_COMPLEMENTOS
ADD CONSTRAINT FK_JOGOSCOMPLEMENTOS_JOGOS
FOREIGN KEY (jogoId)
REFERENCES JOGOS(idJogo)
GO

ALTER TABLE JOGOS_COMPLEMENTOS
ADD CONSTRAINT FK_JOGOSCOMPLEMENTOS_COMPLEMENTOS
FOREIGN KEY (complementoId)
REFERENCES COMPLEMENTOS(idComplemento)
GO

ALTER TABLE JOGOS_COMPLEMENTOS
ADD CONSTRAINT PK_JOGOSCOMPLEMENTOS
PRIMARY KEY (complementoId, jogoId)
GO

ALTER TABLE PEDIDOS
ADD CONSTRAINT FK_PEDIDOS_USUARIOS
FOREIGN KEY (usuarioId)
REFERENCES USUARIOS(idUsuario)
GO

ALTER TABLE PRODUTOS_PEDIDOS
ADD CONSTRAINT FK_PRODUTOSPEDIDO_PEDIDOS
FOREIGN KEY (pedidoId)
REFERENCES PEDIDOS(idPedido)
GO

ALTER TABLE PRODUTOS_PEDIDOS
ADD CONSTRAINT FK_PRODUTOSPEDIDOS_JOGOS
FOREIGN KEY (jogoId)
REFERENCES JOGOS(idJogo)
GO

ALTER TABLE PRODUTOS_PEDIDOS
ADD CONSTRAINT FK_PRODUTOSPEDIDOS_COMPLEMENTOS
FOREIGN KEY (complementoId)
REFERENCES COMPLEMENTOS(idComplemento)
GO

-- INSERTS NAS TABELAS "FIXAS"
INSERT INTO GENEROS VALUES('Casual'),
						  ('Ação'),
						  ('Aventura'),
						  ('Indie'),
						  ('Multijogador Massivo'),
						  ('RPG'),
						  ('Simulação'),
						  ('Estratégia'),
						  ('Corrida')
						  

INSERT INTO TIPOS VALUES('JxJ on-line'),
						('JxJ em rede local(LAN)'),
						('Cooperativo on-line'),
						('Cooperativo em rede local(LAN)'),
						('Multijogador multiplataforma'),
						('Compatibilidade total com controle'),
						('Um jogador'),
						('MMO'),
						('Inclui editor de n�veis'),
						('Compatibilidade parcial com controle'),
						('Cooperativo tela dividida/Compartilhada'),
						('JxJ tela dividida/compart')


INSERT INTO EMPRESAS VALUES('Innersloth'),
						   ('Studio Wildcard'),
						   ('Instinct Games'),
						   ('Efecto Studios'),
						   ('Virtual Basement LLC'),
						   ('Bohemia Interactive'),
						   ('2K Boston'),
						   ('2K Australia'),
						   ('2K'),
						   ('Blind Squirrel'),
						   ('Feral Interactive'),
						   ('2K Marin'),
						   ('2K China'),
						   ('Digital Extremes'),
						   ('Irrational Games'),
						   ('Virtual Programming'),
						   ('Pearl Abyss'),
						   ('Ninja Kiwi'),
						   ('Gearbox Software'),
						   ('Aspyr'),
						   ('Blue Mammoth Games'),
						   ('The Behemoth'),
						   ('Naddic Games'),
						   ('Relic Entertainment'),
						   ('SEGA'),
						   ('QCF Desgin'),
						   ('h.a.n.d., Inc'),
						   ('BANDAI NANCO Entertainment Inc.'),
						   ('BANDAI NAMCO Entertainment'),
						   ('Arkane Studios'),
						   ('Klei Entertainment'),
						   ('Bethesda Softworks'),
						   ('Ubisoft Montreal'),
						   ('Ubisoft Quebec'),
						   ('Ubisoft Toronto'),
						   ('Blue Byte'),
						   ('Ubisoft'),
						   ('Scott Cawthon'),
						   ('Gaggle Studios, Inc.'),
						   ('Rockstar Games'),
						   ('KOG'),
						   ('Supergiant Games'),
						   ('Hoplon'),
						   ('Forthright Entertainment'),
						   ('Panic Art Studios Ltd.'),
						   ('Io-Interactive A/S'),
						   ('Team Cherry'),
						   ('Empyrean'),
						   ('Frozend District'),
						   ('PlayWay S.A.'),
						   ('NetherRealm Studios'),
						   ('High Voltage Software'),
						   ('Warner Bros. Interactive Entertainment'),
						   ('Tate Multimedia'),
						   ('Valve'),
						   ('Tarsier Studios'),
						   ('4A Games'),
						   ('Deep Silver'),
						   ('TaleWorlds Entertainment'),
						   ('Rava Games'),
						   ('CyberConnect2 Co., Ltd.'),
						   ('Moon Studios GmbH'),
						   ('Xbox Game Studuios'),
						   ('Pinoki Games'),
						   ('Kverta'),
						   ('tinyBuild')

INSERT INTO USUARIOS(nome, nick, senha, email, status, tipo) VALUES ('João Acácio', 'JoAca', '25256142', 'JoAca@email.com', 1, 0)
GO