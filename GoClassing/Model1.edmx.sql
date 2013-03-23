
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/17/2012 12:09:12
-- Generated from EDMX file: C:\Users\LayersSss\Documents\Visual Studio 2010\Projects\PsudoSoho\GoClassing\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [gc_localtest];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_gc_course_gc_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_course] DROP CONSTRAINT [FK_gc_course_gc_user];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_feed_gc_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_feed] DROP CONSTRAINT [FK_gc_feed_gc_user];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_friendship_gc_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_friendship] DROP CONSTRAINT [FK_gc_friendship_gc_user];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_friendship_gc_user1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_friendship] DROP CONSTRAINT [FK_gc_friendship_gc_user1];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_msg_gc_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_msg] DROP CONSTRAINT [FK_gc_msg_gc_user];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_note_gc_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_note] DROP CONSTRAINT [FK_gc_note_gc_user];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_paticipate_gc_course]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_paticipate] DROP CONSTRAINT [FK_gc_paticipate_gc_course];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_paticipate_gc_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_paticipate] DROP CONSTRAINT [FK_gc_paticipate_gc_user];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_post_gc_course]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_post] DROP CONSTRAINT [FK_gc_post_gc_course];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_profile_gc_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_profile] DROP CONSTRAINT [FK_gc_profile_gc_user];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_reply_gc_post]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_reply] DROP CONSTRAINT [FK_gc_reply_gc_post];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_reply_gc_user]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_reply] DROP CONSTRAINT [FK_gc_reply_gc_user];
GO
IF OBJECT_ID(N'[dbo].[FK_gc_tag_gc_course]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gc_tag] DROP CONSTRAINT [FK_gc_tag_gc_course];
GO
IF OBJECT_ID(N'[dbo].[FK_gccon_city_gccon_province]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gccon_city] DROP CONSTRAINT [FK_gccon_city_gccon_province];
GO
IF OBJECT_ID(N'[dbo].[FK_gccon_ctype2_gccon_ctype1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gccon_ctype2] DROP CONSTRAINT [FK_gccon_ctype2_gccon_ctype1];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[gc_course]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gc_course];
GO
IF OBJECT_ID(N'[dbo].[gc_feed]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gc_feed];
GO
IF OBJECT_ID(N'[dbo].[gc_friendship]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gc_friendship];
GO
IF OBJECT_ID(N'[dbo].[gc_msg]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gc_msg];
GO
IF OBJECT_ID(N'[dbo].[gc_note]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gc_note];
GO
IF OBJECT_ID(N'[dbo].[gc_paticipate]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gc_paticipate];
GO
IF OBJECT_ID(N'[dbo].[gc_post]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gc_post];
GO
IF OBJECT_ID(N'[dbo].[gc_profile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gc_profile];
GO
IF OBJECT_ID(N'[dbo].[gc_reply]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gc_reply];
GO
IF OBJECT_ID(N'[dbo].[gc_tag]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gc_tag];
GO
IF OBJECT_ID(N'[dbo].[gc_user]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gc_user];
GO
IF OBJECT_ID(N'[dbo].[gccon_city]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gccon_city];
GO
IF OBJECT_ID(N'[dbo].[gccon_coop]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gccon_coop];
GO
IF OBJECT_ID(N'[dbo].[gccon_ctype1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gccon_ctype1];
GO
IF OBJECT_ID(N'[dbo].[gccon_ctype2]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gccon_ctype2];
GO
IF OBJECT_ID(N'[dbo].[gccon_profilename]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gccon_profilename];
GO
IF OBJECT_ID(N'[dbo].[gccon_province]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gccon_province];
GO
IF OBJECT_ID(N'[dbo].[upload_ticket]', 'U') IS NOT NULL
    DROP TABLE [dbo].[upload_ticket];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'gc_course'
CREATE TABLE [dbo].[gc_course] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50)  NOT NULL,
    [type1] nvarchar(50)  NOT NULL,
    [type2] nvarchar(50)  NOT NULL,
    [user_id] int  NOT NULL,
    [canJoin] bit  NOT NULL
);
GO

-- Creating table 'gc_feed'
CREATE TABLE [dbo].[gc_feed] (
    [id] int IDENTITY(1,1) NOT NULL,
    [ftype] nvarchar(50)  NOT NULL,
    [text] nvarchar(max)  NOT NULL,
    [time] datetime  NOT NULL,
    [user_id] int  NOT NULL
);
GO

-- Creating table 'gc_friendship'
CREATE TABLE [dbo].[gc_friendship] (
    [id] int IDENTITY(1,1) NOT NULL,
    [user_id1] int  NOT NULL,
    [user_id2] int  NOT NULL,
    [accepted] bit  NOT NULL
);
GO

-- Creating table 'gc_msg'
CREATE TABLE [dbo].[gc_msg] (
    [id] int IDENTITY(1,1) NOT NULL,
    [user_id] int  NOT NULL,
    [fromid] int  NOT NULL,
    [nexturl] nvarchar(max)  NULL,
    [content] nvarchar(max)  NOT NULL,
    [read] bit  NOT NULL,
    [time] datetime  NOT NULL,
    [mailed] bit  NOT NULL
);
GO

-- Creating table 'gc_note'
CREATE TABLE [dbo].[gc_note] (
    [id] int IDENTITY(1,1) NOT NULL,
    [privateuser_id] int  NOT NULL,
    [user_id] int  NOT NULL,
    [fromuser_id] int  NOT NULL,
    [content] nvarchar(300)  NOT NULL,
    [time] datetime  NOT NULL
);
GO

-- Creating table 'gc_paticipate'
CREATE TABLE [dbo].[gc_paticipate] (
    [id] int IDENTITY(1,1) NOT NULL,
    [course_id] int  NOT NULL,
    [overdue] datetime  NULL,
    [approved] bit  NOT NULL,
    [user_id] int  NOT NULL
);
GO

-- Creating table 'gc_post'
CREATE TABLE [dbo].[gc_post] (
    [id] int IDENTITY(1,1) NOT NULL,
    [tag] nvarchar(50)  NULL,
    [title] nvarchar(50)  NOT NULL,
    [course_id] int  NULL,
    [ftype] nvarchar(50)  NOT NULL,
    [time] datetime  NOT NULL,
    [sort] int  NOT NULL
);
GO

-- Creating table 'gc_profile'
CREATE TABLE [dbo].[gc_profile] (
    [id] int IDENTITY(1,1) NOT NULL,
    [user_id] int  NOT NULL,
    [name] nvarchar(50)  NOT NULL,
    [sort] int  NULL,
    [value] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'gc_reply'
CREATE TABLE [dbo].[gc_reply] (
    [id] int IDENTITY(1,1) NOT NULL,
    [time] datetime  NOT NULL,
    [user_id] int  NOT NULL,
    [ext] nvarchar(50)  NULL,
    [ftype] nvarchar(50)  NULL,
    [content] nvarchar(1000)  NOT NULL,
    [post_id] int  NOT NULL,
    [public] bit  NOT NULL,
    [filename] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'gc_tag'
CREATE TABLE [dbo].[gc_tag] (
    [id] int IDENTITY(1,1) NOT NULL,
    [tag] nvarchar(50)  NOT NULL,
    [course_id] int  NOT NULL,
    [sort] int  NOT NULL,
    [leftcol] bit  NOT NULL,
    [canReply] bit  NOT NULL,
    [canGuestView] bit  NOT NULL
);
GO

-- Creating table 'gc_user'
CREATE TABLE [dbo].[gc_user] (
    [id] int IDENTITY(1,1) NOT NULL,
    [sex] bit  NULL,
    [originalusername] nvarchar(50)  NOT NULL,
    [username] nvarchar(50)  NOT NULL,
    [email] nvarchar(50)  NOT NULL,
    [password] nvarchar(50)  NOT NULL,
    [admin] bit  NOT NULL,
    [flagsettings] int  NOT NULL,
    [mailverified] bit  NOT NULL,
    [truename] nvarchar(50)  NULL,
    [uprovince] nvarchar(50)  NULL,
    [ucity] nvarchar(50)  NULL,
    [timeCreation] datetime  NOT NULL,
    [timeLastLogon] datetime  NOT NULL,
    [timeLastPasswordChange] datetime  NOT NULL,
    [timeLastActivity] datetime  NOT NULL,
    [pwdQuestion] nvarchar(50)  NULL,
    [pwdAnswer] nvarchar(50)  NULL,
    [coop] nvarchar(50)  NULL,
    [coopSort] int  NULL,
    [spaceTotal] bigint  NOT NULL,
    [spaceUsed] bigint  NOT NULL,
    [msgType] int  NOT NULL,
    [msgClock] int  NOT NULL
);
GO

-- Creating table 'gccon_city'
CREATE TABLE [dbo].[gccon_city] (
    [city] nvarchar(50)  NOT NULL,
    [province] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'gccon_coop'
CREATE TABLE [dbo].[gccon_coop] (
    [coop] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'gccon_ctype1'
CREATE TABLE [dbo].[gccon_ctype1] (
    [type] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'gccon_ctype2'
CREATE TABLE [dbo].[gccon_ctype2] (
    [type] nvarchar(50)  NOT NULL,
    [ctype1_type] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'gccon_profilename'
CREATE TABLE [dbo].[gccon_profilename] (
    [name] nvarchar(50)  NOT NULL,
    [defaultSort] int  NOT NULL
);
GO

-- Creating table 'gccon_province'
CREATE TABLE [dbo].[gccon_province] (
    [province] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'upload_ticket'
CREATE TABLE [dbo].[upload_ticket] (
    [id] uniqueidentifier  NOT NULL,
    [path] nvarchar(50)  NOT NULL,
    [expires] datetime  NOT NULL,
    [limit] int  NOT NULL,
    [space_user_id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'gc_course'
ALTER TABLE [dbo].[gc_course]
ADD CONSTRAINT [PK_gc_course]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'gc_feed'
ALTER TABLE [dbo].[gc_feed]
ADD CONSTRAINT [PK_gc_feed]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'gc_friendship'
ALTER TABLE [dbo].[gc_friendship]
ADD CONSTRAINT [PK_gc_friendship]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'gc_msg'
ALTER TABLE [dbo].[gc_msg]
ADD CONSTRAINT [PK_gc_msg]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'gc_note'
ALTER TABLE [dbo].[gc_note]
ADD CONSTRAINT [PK_gc_note]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'gc_paticipate'
ALTER TABLE [dbo].[gc_paticipate]
ADD CONSTRAINT [PK_gc_paticipate]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'gc_post'
ALTER TABLE [dbo].[gc_post]
ADD CONSTRAINT [PK_gc_post]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'gc_profile'
ALTER TABLE [dbo].[gc_profile]
ADD CONSTRAINT [PK_gc_profile]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'gc_reply'
ALTER TABLE [dbo].[gc_reply]
ADD CONSTRAINT [PK_gc_reply]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'gc_tag'
ALTER TABLE [dbo].[gc_tag]
ADD CONSTRAINT [PK_gc_tag]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'gc_user'
ALTER TABLE [dbo].[gc_user]
ADD CONSTRAINT [PK_gc_user]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [city] in table 'gccon_city'
ALTER TABLE [dbo].[gccon_city]
ADD CONSTRAINT [PK_gccon_city]
    PRIMARY KEY CLUSTERED ([city] ASC);
GO

-- Creating primary key on [coop] in table 'gccon_coop'
ALTER TABLE [dbo].[gccon_coop]
ADD CONSTRAINT [PK_gccon_coop]
    PRIMARY KEY CLUSTERED ([coop] ASC);
GO

-- Creating primary key on [type] in table 'gccon_ctype1'
ALTER TABLE [dbo].[gccon_ctype1]
ADD CONSTRAINT [PK_gccon_ctype1]
    PRIMARY KEY CLUSTERED ([type] ASC);
GO

-- Creating primary key on [type] in table 'gccon_ctype2'
ALTER TABLE [dbo].[gccon_ctype2]
ADD CONSTRAINT [PK_gccon_ctype2]
    PRIMARY KEY CLUSTERED ([type] ASC);
GO

-- Creating primary key on [name] in table 'gccon_profilename'
ALTER TABLE [dbo].[gccon_profilename]
ADD CONSTRAINT [PK_gccon_profilename]
    PRIMARY KEY CLUSTERED ([name] ASC);
GO

-- Creating primary key on [province] in table 'gccon_province'
ALTER TABLE [dbo].[gccon_province]
ADD CONSTRAINT [PK_gccon_province]
    PRIMARY KEY CLUSTERED ([province] ASC);
GO

-- Creating primary key on [id] in table 'upload_ticket'
ALTER TABLE [dbo].[upload_ticket]
ADD CONSTRAINT [PK_upload_ticket]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [user_id] in table 'gc_course'
ALTER TABLE [dbo].[gc_course]
ADD CONSTRAINT [FK_gc_course_gc_user]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[gc_user]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_course_gc_user'
CREATE INDEX [IX_FK_gc_course_gc_user]
ON [dbo].[gc_course]
    ([user_id]);
GO

-- Creating foreign key on [course_id] in table 'gc_paticipate'
ALTER TABLE [dbo].[gc_paticipate]
ADD CONSTRAINT [FK_gc_paticipate_gc_course]
    FOREIGN KEY ([course_id])
    REFERENCES [dbo].[gc_course]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_paticipate_gc_course'
CREATE INDEX [IX_FK_gc_paticipate_gc_course]
ON [dbo].[gc_paticipate]
    ([course_id]);
GO

-- Creating foreign key on [course_id] in table 'gc_post'
ALTER TABLE [dbo].[gc_post]
ADD CONSTRAINT [FK_gc_post_gc_course]
    FOREIGN KEY ([course_id])
    REFERENCES [dbo].[gc_course]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_post_gc_course'
CREATE INDEX [IX_FK_gc_post_gc_course]
ON [dbo].[gc_post]
    ([course_id]);
GO

-- Creating foreign key on [course_id] in table 'gc_tag'
ALTER TABLE [dbo].[gc_tag]
ADD CONSTRAINT [FK_gc_tag_gc_course]
    FOREIGN KEY ([course_id])
    REFERENCES [dbo].[gc_course]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_tag_gc_course'
CREATE INDEX [IX_FK_gc_tag_gc_course]
ON [dbo].[gc_tag]
    ([course_id]);
GO

-- Creating foreign key on [user_id] in table 'gc_feed'
ALTER TABLE [dbo].[gc_feed]
ADD CONSTRAINT [FK_gc_feed_gc_user]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[gc_user]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_feed_gc_user'
CREATE INDEX [IX_FK_gc_feed_gc_user]
ON [dbo].[gc_feed]
    ([user_id]);
GO

-- Creating foreign key on [user_id1] in table 'gc_friendship'
ALTER TABLE [dbo].[gc_friendship]
ADD CONSTRAINT [FK_gc_friendship_gc_user]
    FOREIGN KEY ([user_id1])
    REFERENCES [dbo].[gc_user]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_friendship_gc_user'
CREATE INDEX [IX_FK_gc_friendship_gc_user]
ON [dbo].[gc_friendship]
    ([user_id1]);
GO

-- Creating foreign key on [user_id2] in table 'gc_friendship'
ALTER TABLE [dbo].[gc_friendship]
ADD CONSTRAINT [FK_gc_friendship_gc_user1]
    FOREIGN KEY ([user_id2])
    REFERENCES [dbo].[gc_user]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_friendship_gc_user1'
CREATE INDEX [IX_FK_gc_friendship_gc_user1]
ON [dbo].[gc_friendship]
    ([user_id2]);
GO

-- Creating foreign key on [user_id] in table 'gc_msg'
ALTER TABLE [dbo].[gc_msg]
ADD CONSTRAINT [FK_gc_msg_gc_user]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[gc_user]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_msg_gc_user'
CREATE INDEX [IX_FK_gc_msg_gc_user]
ON [dbo].[gc_msg]
    ([user_id]);
GO

-- Creating foreign key on [user_id] in table 'gc_note'
ALTER TABLE [dbo].[gc_note]
ADD CONSTRAINT [FK_gc_note_gc_user]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[gc_user]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_note_gc_user'
CREATE INDEX [IX_FK_gc_note_gc_user]
ON [dbo].[gc_note]
    ([user_id]);
GO

-- Creating foreign key on [user_id] in table 'gc_paticipate'
ALTER TABLE [dbo].[gc_paticipate]
ADD CONSTRAINT [FK_gc_paticipate_gc_user]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[gc_user]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_paticipate_gc_user'
CREATE INDEX [IX_FK_gc_paticipate_gc_user]
ON [dbo].[gc_paticipate]
    ([user_id]);
GO

-- Creating foreign key on [post_id] in table 'gc_reply'
ALTER TABLE [dbo].[gc_reply]
ADD CONSTRAINT [FK_gc_reply_gc_post]
    FOREIGN KEY ([post_id])
    REFERENCES [dbo].[gc_post]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_reply_gc_post'
CREATE INDEX [IX_FK_gc_reply_gc_post]
ON [dbo].[gc_reply]
    ([post_id]);
GO

-- Creating foreign key on [user_id] in table 'gc_profile'
ALTER TABLE [dbo].[gc_profile]
ADD CONSTRAINT [FK_gc_profile_gc_user]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[gc_user]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_profile_gc_user'
CREATE INDEX [IX_FK_gc_profile_gc_user]
ON [dbo].[gc_profile]
    ([user_id]);
GO

-- Creating foreign key on [user_id] in table 'gc_reply'
ALTER TABLE [dbo].[gc_reply]
ADD CONSTRAINT [FK_gc_reply_gc_user]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[gc_user]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gc_reply_gc_user'
CREATE INDEX [IX_FK_gc_reply_gc_user]
ON [dbo].[gc_reply]
    ([user_id]);
GO

-- Creating foreign key on [province] in table 'gccon_city'
ALTER TABLE [dbo].[gccon_city]
ADD CONSTRAINT [FK_gccon_city_gccon_province]
    FOREIGN KEY ([province])
    REFERENCES [dbo].[gccon_province]
        ([province])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gccon_city_gccon_province'
CREATE INDEX [IX_FK_gccon_city_gccon_province]
ON [dbo].[gccon_city]
    ([province]);
GO

-- Creating foreign key on [ctype1_type] in table 'gccon_ctype2'
ALTER TABLE [dbo].[gccon_ctype2]
ADD CONSTRAINT [FK_gccon_ctype2_gccon_ctype1]
    FOREIGN KEY ([ctype1_type])
    REFERENCES [dbo].[gccon_ctype1]
        ([type])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_gccon_ctype2_gccon_ctype1'
CREATE INDEX [IX_FK_gccon_ctype2_gccon_ctype1]
ON [dbo].[gccon_ctype2]
    ([ctype1_type]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------