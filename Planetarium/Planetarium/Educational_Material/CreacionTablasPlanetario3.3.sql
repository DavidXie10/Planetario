CREATE TABLE Topico
(	nombrePK			NVARCHAR(50)		PRIMARY KEY NOT NULL,
	categoria			NVARCHAR(50)		NOT NULL,
);

CREATE TABLE Visitante
(	cedulaPK			NVARCHAR(15)		NOT NULL PRIMARY KEY,
	correo				NVARCHAR(100)		UNIQUE,
	nivelEducativo		NVARCHAR(100)		NULL,
	fechaNacimiento		DATE				NOT NULL,
	genero				NCHAR(1)			NOT NULL DEFAULT 'N',
	lugarDeResidencia	NVARCHAR(100)		NULL,
	nombreCompleto		NVARCHAR(50)		NOT NULL 
);

CREATE TABLE Funcionario
(	cedulaPK			NVARCHAR(15)		NOT NULL PRIMARY KEY,
	ocupacion			NVARCHAR(100)		NOT NULL,
	titulosAcademicos	NVARCHAR(200)		NULL,
	correo				NVARCHAR(100)		UNIQUE,
	nombre				NVARCHAR(50)		NOT NULL,
	apellido			NVARCHAR(50)		NOT NULL,
	frase				NVARCHAR(200)		NULL,
	genero				NCHAR(1)			NOT NULL DEFAULT 'N',
	fechaInicioEmpleo	DATE				NOT NULL,
	fechaNacimiento		DATE				NOT NULL,
	telefono			INT					UNIQUE,
	banderaColaborador	BIT					NOT NULL DEFAULT 0,
	areaExpertiz		NVARCHAR(200)		NULL,
	banderaCoordinador	BIT					NOT NULL DEFAULT 0,
	banderaEducador		BIT					NOT NULL DEFAULT 0,
	lugarDeResidencia	NVARCHAR(100)		NULL,
	rutaFotoPerfil 		NVARCHAR(100)		NULL
);

CREATE TABLE PreguntaFrecuente 
(	idPK						INT					IDENTITY(1,1) NOT NULL PRIMARY KEY,
	pregunta					NVARCHAR(500)		NOT NULL,
	respuesta					NVARCHAR(500)		NOT NULL,
	cedulaFK					NVARCHAR(15)		NOT NULL DEFAULT '0',
	
	
	CONSTRAINT FK_Colaborador_PreguntaFrecuente FOREIGN KEY (cedulaFK)
		REFERENCES Funcionario(cedulaPK)
			ON DELETE	SET DEFAULT
			ON UPDATE	CASCADE
	/*TO-DO: Trigger para revisar que la cedulaFK pertenezca a un colaborador*/
);

CREATE TABLE PreguntaFrecuentePerteneceATopico
(	idPreguntaFrecuentePKFK			INT					NOT NULL,
	nombreTopicoPKFK				NVARCHAR(50)		NOT NULL,

	CONSTRAINT PK_PreguntaFrecuentePerteneceATopico PRIMARY KEY (idPreguntaFrecuentePKFK, nombreTopicoPKFK),

	CONSTRAINT FK_PreguntaFrecuente_PreguntaFrecuentePerteneceATopico FOREIGN KEY (idPreguntaFrecuentePKFK)
		REFERENCES PreguntaFrecuente(idPreguntaPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE,
	CONSTRAINT FK_Topico_PreguntaFrecuentePerteneceATopico FOREIGN KEY (nombreTopicoPKFK)
		REFERENCES Topico(nombrePK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE
);

CREATE TABLE ActividadEducativa
(	tituloPK			NVARCHAR(50)		NOT NULL,
	fechaInicioPK		DATE				NOT NULL,
	duracion			INT					NOT NULL,
	capacidadMaxima		INT					NOT NULL,
	precio				FLOAT				NOT NULL DEFAULT 0,
	descripcion			NVARCHAR(200)		NOT NULL DEFAULT '',
	nivelComplejidad	NVARCHAR(50)		NULL,
	estadoRevision		SMALLINT			NOT NULL,
	modalidad			NVARCHAR(50)		NOT NULL,
	banderaVirtual		BIT					NOT NULL DEFAULT 0,
	enlace				NVARCHAR(100)		NULL,
	banderaPresencial	BIT					NOT NULL DEFAULT 0,
	cedulaFK			NVARCHAR(15)		NOT NULL DEFAULT '0'

	CONSTRAINT PK_ActividadEducativa PRIMARY KEY (tituloPK, fechaInicioPK),
	CONSTRAINT FK_Educador_ActividadEducativa FOREIGN KEY (cedulaFK)
		REFERENCES Funcionario(cedulaPK)
			ON DELETE	SET DEFAULT
			ON UPDATE	CASCADE

);


CREATE TABLE ActividadEducativaPerteneceATopico
(	tituloPKFK			NVARCHAR(50)		NOT NULL,
	fechaInicioPKFK		DATE				NOT NULL,
	nombreTopicoPKFK	NVARCHAR(50)		NOT NULL,

	CONSTRAINT PK_ActividadEducativaPerteneceATopico PRIMARY KEY (tituloPKFK, fechaInicioPKFK, nombreTopicoPKFK),

	CONSTRAINT FK_ActividadEducativa_ActividadEducativaPerteneceATopico FOREIGN KEY (tituloPKFK, fechaInicioPKFK)
		REFERENCES ActividadEducativa(tituloPK, fechaInicioPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE,
	CONSTRAINT FK_Topico_ActividadEducativaPerteneceATopico FOREIGN KEY (nombreTopicoPKFK)
		REFERENCES Topico(nombrePK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE
);


CREATE TABLE PublicoMeta
(	tituloPKFK			NVARCHAR(50)		NOT NULL,
	fechaInicioPKFK		DATE				NOT NULL,
	publicoMetaPK		NVARCHAR(50)		NOT NULL,

	CONSTRAINT PK_PublicoMeta PRIMARY KEY (tituloPKFK, fechaInicioPKFK, publicoMetaPK),
	CONSTRAINT FK_ActividadEducativa_PublicoMeta FOREIGN KEY (tituloPKFK,fechaInicioPKFK)
		REFERENCES ActividadEducativa(tituloPK,fechaInicioPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE
);

CREATE TABLE Noticia
(	tituloPK			NVARCHAR(200)		NOT NULL PRIMARY KEY,
	resumen				NVARCHAR(500)		NOT NULL,
	fechaPublicacion	DATE				NOT NULL,
	cedulaFK			NVARCHAR(15)		NOT NULL DEFAULT '0',
	contenido			NVARCHAR(MAX)		NOT NULL,
	autor 				NVARCHAR(50)		NOT NULL

	CONSTRAINT FK_Coordinador_Noticia FOREIGN KEY (cedulaFK)
		REFERENCES Funcionario(cedulaPK)
			ON DELETE	SET DEFAULT
			ON UPDATE	CASCADE
	/*Falta hacer trigger para que cedulaPK de funcioanrio solamente corresponda a Coordinador*/
);

CREATE TABLE NoticiaPerteneceATopico
(	tituloPKFK			NVARCHAR(200)		NOT NULL,
	nombreTopicoPKFK	NVARCHAR(50)		NOT NULL,

	CONSTRAINT PK_NoticiaPerteneceATopico PRIMARY KEY (tituloPKFK, nombreTopicoPKFK),

	CONSTRAINT FK_Noticia_NoticiaPerteneceATopico FOREIGN KEY (tituloPKFK)
		REFERENCES Noticia(tituloPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE,
	CONSTRAINT FK_Topico_NoticiaPerteneceATopico FOREIGN KEY (nombreTopicoPKFK)
		REFERENCES Topico(nombrePK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE
);

CREATE TABLE ImagenPerteneceANoticia
(	tituloPKFK			NVARCHAR(200)		NOT NULL,
	referenciaImagenPK	NVARCHAR(100)		NOT NULL,

	CONSTRAINT PK_ImagenPerteneceANoticia PRIMARY KEY (tituloPKFK, referenciaImagenPK),

	CONSTRAINT FK_Noticia_ImagenPerteneceANoticia FOREIGN KEY (tituloPKFK)
		REFERENCES Noticia(tituloPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE
);

CREATE TABLE AsignarAsiento
(	tituloPKFK			NVARCHAR(50)		NOT NULL,
	fechaInicioPKFK		DATE				NOT NULL,
	cedulaPKFK			NVARCHAR(15)		NOT NULL,
	numeroAsiento		INT					NOT NULL,

	CONSTRAINT PK_AsignarAsiento PRIMARY KEY (tituloPKFK,fechaInicioPKFK,cedulaPKFK),
	/*Falta trigger para verificar si el funcioanrio es un educador*/
	CONSTRAINT FK_ActividadEducativa_AsignarAsiento FOREIGN KEY (tituloPKFK,fechaInicioPKFK)
		REFERENCES ActividadEducativa(tituloPK, fechaInicioPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE,
	CONSTRAINT FK_Visitante_AsignarAsiento FOREIGN KEY (cedulaPKFK)
		REFERENCES Visitante(cedulaPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE
);

CREATE TABLE MaterialEducativo
(	autorPK				NVARCHAR(50)		NOT NULL,
	tituloPK			NVARCHAR(200)		NOT NULL,
	fechaPublicacion	DATE				NOT NULL,

	CONSTRAINT PK_MaterialEducativo PRIMARY KEY (autorPK, tituloPK),
);

CREATE TABLE MaterialEducativoPerteneceATopico
(	autorPKFK			NVARCHAR(50)		NOT NULL,
	tituloPKFK			NVARCHAR(200)		NOT NULL,
	nombreTopicoPKFK	NVARCHAR(50)		NOT NULL,

	CONSTRAINT PK_MaterialEducativoPerteneceATopico PRIMARY KEY (autorPKFK, tituloPKFK,nombreTopicoPKFK),

	CONSTRAINT FK_MaterialEducativo_MaterialEducativoPerteneceATopico FOREIGN KEY (autorPKFK, tituloPKFK)
		REFERENCES MaterialEducativo(autorPK, tituloPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE,
	CONSTRAINT FK_Topico_MaterialEducativoPerteneceATopico FOREIGN KEY (nombreTopicoPKFK)
		REFERENCES Topico(nombrePK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE
);

CREATE TABLE NombreArchivoMaterialEducativo
(	autorPKFK			NVARCHAR(50)		NOT NULL,
	tituloPKFK			NVARCHAR(200)		NOT NULL,
	nombreArchivoPK		NVARCHAR(200)		NOT NULL

	CONSTRAINT PK_NombreArchivoMaterialEducativo PRIMARY KEY (autorPKFK, tituloPKFK, nombreArchivoPK),
	CONSTRAINT FK_MaterialEducativo_NombreArchivoMaterialEducativo FOREIGN KEY (autorPKFK,tituloPKFK)
		REFERENCES MaterialEducativo(autorPK,tituloPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE
);

CREATE TABLE Inscribirse
(	cedulaPKFK			NVARCHAR(15)		NOT NULL,
	tituloPKFK			NVARCHAR(50)		NOT NULL,
	fechaInicioPKFK		DATE				NOT NULL

	CONSTRAINT PK_Inscribirse PRIMARY KEY (cedulaPKFK, tituloPKFK, fechaInicioPKFK),
	CONSTRAINT FK_Visitante_Inscribirse FOREIGN KEY (cedulaPKFK)
		REFERENCES Visitante(cedulaPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE,
	CONSTRAINT FK_ActividadEducativa_Inscribirse FOREIGN KEY (tituloPKFK, fechaInicioPKFK)
		REFERENCES ActividadEducativa(tituloPK, fechaInicioPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE
);

CREATE TABLE Ofrecer
(	cedulaPKFK			NVARCHAR(15)		NOT NULL,
	tituloActividadPKFK	NVARCHAR(50)		NOT NULL,
	fechaInicioPKFK		DATE				NOT NULL,
	tituloMaterialPKFK	NVARCHAR(200)		NOT NULL,
	autorPKFK			NVARCHAR(50)		NOT NULL,

	CONSTRAINT PK_Ofrecer PRIMARY KEY (cedulaPKFK, tituloActividadPKFK, fechaInicioPKFK, tituloMaterialPKFK, autorPKFK),
	CONSTRAINT FK_Educador_Ofrecer FOREIGN KEY (cedulaPKFK)
		REFERENCES Funcionario(cedulaPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE,
	/*Falta assert para verificar si el funcioanrio es un educador*/
	CONSTRAINT FK_ActividadEducativa_Ofrecer FOREIGN KEY (tituloActividadPKFK, fechaInicioPKFK)
		REFERENCES ActividadEducativa(tituloPK, fechaInicioPK)
			ON DELETE	NO ACTION
			ON UPDATE	NO ACTION,
	CONSTRAINT FK_MaterialEducativo_Ofrecer FOREIGN KEY (autorPKFK,tituloMaterialPKFK)
		REFERENCES MaterialEducativo(autorPK,tituloPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE
);

CREATE TABLE Idioma
(	cedulaPK			NVARCHAR(15)		NOT NULL,
	idiomaPK			NVARCHAR(50)		NOT NULL,

	CONSTRAINT PK_Idioma PRIMARY KEY (cedulaPK,idiomaPK),
	CONSTRAINT FK_Colaborador_Idioma FOREIGN KEY (cedulaPK)
		REFERENCES Funcionario(cedulaPK)
			ON DELETE	CASCADE
			ON UPDATE	CASCADE
);
