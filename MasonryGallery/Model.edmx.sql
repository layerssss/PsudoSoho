
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/15/2012 16:45:40
-- Generated from EDMX file: C:\Users\LayersSss\Documents\Visual Studio 2010\Projects\PsudoSoho\MasonryGallery\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MG];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MG_comment_MG_photo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MG_comment] DROP CONSTRAINT [FK_MG_comment_MG_photo];
GO
IF OBJECT_ID(N'[dbo].[FK_MG_description_MG_photo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MG_description] DROP CONSTRAINT [FK_MG_description_MG_photo];
GO
IF OBJECT_ID(N'[dbo].[FK_MG_langusing_MG_gallery]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MG_langusing] DROP CONSTRAINT [FK_MG_langusing_MG_gallery];
GO
IF OBJECT_ID(N'[dbo].[FK_MG_langusing_MG_lang]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MG_langusing] DROP CONSTRAINT [FK_MG_langusing_MG_lang];
GO
IF OBJECT_ID(N'[dbo].[FK_MG_photo_MG_gallery]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MG_album] DROP CONSTRAINT [FK_MG_photo_MG_gallery];
GO
IF OBJECT_ID(N'[dbo].[FK_MG_subpic_MG_album]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MG_subpic] DROP CONSTRAINT [FK_MG_subpic_MG_album];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[MG_album]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MG_album];
GO
IF OBJECT_ID(N'[dbo].[MG_comment]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MG_comment];
GO
IF OBJECT_ID(N'[dbo].[MG_description]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MG_description];
GO
IF OBJECT_ID(N'[dbo].[MG_gallery]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MG_gallery];
GO
IF OBJECT_ID(N'[dbo].[MG_lang]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MG_lang];
GO
IF OBJECT_ID(N'[dbo].[MG_langusing]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MG_langusing];
GO
IF OBJECT_ID(N'[dbo].[MG_subpic]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MG_subpic];
GO
IF OBJECT_ID(N'[dbo].[MG_theme]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MG_theme];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'MG_album'
CREATE TABLE [dbo].[MG_album] (
    [id] int IDENTITY(1,1) NOT NULL,
    [sort] int  NOT NULL,
    [showWidth] int  NOT NULL,
    [thumbWidth] int  NOT NULL,
    [gallery_id] int  NOT NULL,
    [mainpicurl] nvarchar(max)  NOT NULL,
    [mainpicWidth] int  NOT NULL,
    [mainpicHeight] int  NOT NULL,
    [subpicWidth] int  NOT NULL,
    [YUPOO_photoId] nvarchar(50)  NOT NULL,
    [mainpicurl_origin] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MG_comment'
CREATE TABLE [dbo].[MG_comment] (
    [id] int IDENTITY(1,1) NOT NULL,
    [content] nvarchar(100)  NOT NULL,
    [time] datetime  NOT NULL,
    [name] nvarchar(50)  NOT NULL,
    [avatar] nvarchar(100)  NOT NULL,
    [album_id] int  NOT NULL
);
GO

-- Creating table 'MG_description'
CREATE TABLE [dbo].[MG_description] (
    [id] int IDENTITY(1,1) NOT NULL,
    [lang_code] nvarchar(10)  NOT NULL,
    [album_id] int  NOT NULL,
    [title] nvarchar(50)  NOT NULL,
    [content] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'MG_gallery'
CREATE TABLE [dbo].[MG_gallery] (
    [id] int IDENTITY(1,1) NOT NULL,
    [username] nvarchar(50)  NOT NULL,
    [password] nvarchar(50)  NOT NULL,
    [domainname] nvarchar(128)  NOT NULL,
    [default_lang_code] nvarchar(10)  NOT NULL,
    [width] int  NOT NULL,
    [border] int  NOT NULL,
    [margin] int  NOT NULL,
    [bodyWidth] int  NULL,
    [color1] nchar(6)  NOT NULL,
    [color2] nchar(6)  NOT NULL,
    [color3] nchar(6)  NOT NULL,
    [type] nvarchar(50)  NOT NULL,
    [YUPOO_albumId] nvarchar(50)  NOT NULL,
    [opacity] float  NOT NULL
);
GO

-- Creating table 'MG_lang'
CREATE TABLE [dbo].[MG_lang] (
    [string] nvarchar(50)  NOT NULL,
    [code] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'MG_langusing'
CREATE TABLE [dbo].[MG_langusing] (
    [id] int IDENTITY(1,1) NOT NULL,
    [gallery_id] int  NOT NULL,
    [lang_code] nvarchar(10)  NOT NULL,
    [titile] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'MG_subpic'
CREATE TABLE [dbo].[MG_subpic] (
    [id] int IDENTITY(1,1) NOT NULL,
    [url] nvarchar(max)  NOT NULL,
    [album_id] int  NOT NULL,
    [sort] int  NOT NULL,
    [width] int  NOT NULL,
    [height] int  NOT NULL,
    [YUPOO_photoId] nvarchar(50)  NOT NULL,
    [url_origin] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MG_theme'
CREATE TABLE [dbo].[MG_theme] (
    [id] int IDENTITY(1,1) NOT NULL,
    [color1] varchar(6)  NOT NULL,
    [color2] varchar(6)  NOT NULL,
    [color3] varchar(6)  NOT NULL,
    [sort] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'MG_album'
ALTER TABLE [dbo].[MG_album]
ADD CONSTRAINT [PK_MG_album]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'MG_comment'
ALTER TABLE [dbo].[MG_comment]
ADD CONSTRAINT [PK_MG_comment]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'MG_description'
ALTER TABLE [dbo].[MG_description]
ADD CONSTRAINT [PK_MG_description]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'MG_gallery'
ALTER TABLE [dbo].[MG_gallery]
ADD CONSTRAINT [PK_MG_gallery]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [code] in table 'MG_lang'
ALTER TABLE [dbo].[MG_lang]
ADD CONSTRAINT [PK_MG_lang]
    PRIMARY KEY CLUSTERED ([code] ASC);
GO

-- Creating primary key on [id] in table 'MG_langusing'
ALTER TABLE [dbo].[MG_langusing]
ADD CONSTRAINT [PK_MG_langusing]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'MG_subpic'
ALTER TABLE [dbo].[MG_subpic]
ADD CONSTRAINT [PK_MG_subpic]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'MG_theme'
ALTER TABLE [dbo].[MG_theme]
ADD CONSTRAINT [PK_MG_theme]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [album_id] in table 'MG_comment'
ALTER TABLE [dbo].[MG_comment]
ADD CONSTRAINT [FK_MG_comment_MG_photo]
    FOREIGN KEY ([album_id])
    REFERENCES [dbo].[MG_album]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MG_comment_MG_photo'
CREATE INDEX [IX_FK_MG_comment_MG_photo]
ON [dbo].[MG_comment]
    ([album_id]);
GO

-- Creating foreign key on [album_id] in table 'MG_description'
ALTER TABLE [dbo].[MG_description]
ADD CONSTRAINT [FK_MG_description_MG_photo]
    FOREIGN KEY ([album_id])
    REFERENCES [dbo].[MG_album]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MG_description_MG_photo'
CREATE INDEX [IX_FK_MG_description_MG_photo]
ON [dbo].[MG_description]
    ([album_id]);
GO

-- Creating foreign key on [gallery_id] in table 'MG_album'
ALTER TABLE [dbo].[MG_album]
ADD CONSTRAINT [FK_MG_photo_MG_gallery]
    FOREIGN KEY ([gallery_id])
    REFERENCES [dbo].[MG_gallery]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MG_photo_MG_gallery'
CREATE INDEX [IX_FK_MG_photo_MG_gallery]
ON [dbo].[MG_album]
    ([gallery_id]);
GO

-- Creating foreign key on [album_id] in table 'MG_subpic'
ALTER TABLE [dbo].[MG_subpic]
ADD CONSTRAINT [FK_MG_subpic_MG_album]
    FOREIGN KEY ([album_id])
    REFERENCES [dbo].[MG_album]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MG_subpic_MG_album'
CREATE INDEX [IX_FK_MG_subpic_MG_album]
ON [dbo].[MG_subpic]
    ([album_id]);
GO

-- Creating foreign key on [gallery_id] in table 'MG_langusing'
ALTER TABLE [dbo].[MG_langusing]
ADD CONSTRAINT [FK_MG_langusing_MG_gallery]
    FOREIGN KEY ([gallery_id])
    REFERENCES [dbo].[MG_gallery]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MG_langusing_MG_gallery'
CREATE INDEX [IX_FK_MG_langusing_MG_gallery]
ON [dbo].[MG_langusing]
    ([gallery_id]);
GO

-- Creating foreign key on [lang_code] in table 'MG_langusing'
ALTER TABLE [dbo].[MG_langusing]
ADD CONSTRAINT [FK_MG_langusing_MG_lang]
    FOREIGN KEY ([lang_code])
    REFERENCES [dbo].[MG_lang]
        ([code])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MG_langusing_MG_lang'
CREATE INDEX [IX_FK_MG_langusing_MG_lang]
ON [dbo].[MG_langusing]
    ([lang_code]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------