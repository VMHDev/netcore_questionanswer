USE [master];
DECLARE @kill varchar(8000) = '';  
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), req_spid) + ';'  
FROM master.dbo.syslockinfo
WHERE rsc_type = 2
AND rsc_dbid  = db_id('QA')

EXEC(@kill);

IF EXISTS(SELECT * FROM SYS.DATABASES WHERE NAME = 'QA')
BEGIN
	USE [master];
	DROP DATABASE QA
END
GO
--=========================================================================================================================================================
USE [master];
IF NOT EXISTS(SELECT * FROM SYS.DATABASES WHERE NAME = 'QA')
BEGIN
	CREATE DATABASE QA 
	--ON (NAME = N'QA', 
	--	FILENAME = N'D:\mdf\QA_20201218.mdf')
	--LOG ON (NAME = N'QA_log', 
	--		FILENAME = N'D:\mdf\QA_20201218.ldf')
END
GO
--IF EXISTS(SELECT * FROM SYS.DATABASES WHERE NAME = 'QA')
--BEGIN
--	ALTER DATABASE QA COLLATE Japanese_CI_AS 
--END
--GO
IF EXISTS(SELECT * FROM SYS.DATABASES WHERE NAME = 'QA')
BEGIN
	USE QA
END
GO
--=========================================================================================================================================================
--=========================================================================================================================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT TOP 1 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tag')
BEGIN
CREATE TABLE [dbo].[Tag](
	[tag_id] [bigint] IDENTITY(1,1) NOT NULL,
	[tag_name] [nvarchar](max) NOT NULL
CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[tag_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
--=========================================================================================================================================================
--=========================================================================================================================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT TOP 1 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Question')
BEGIN
CREATE TABLE [dbo].[Question](
	[question_id] [bigint] IDENTITY(1,1) NOT NULL,
	[question_content] [nvarchar](max) NOT NULL,
	[question_vote] [bigint] NOT NULL,
	[question_tag] [bigint] NULL,
	[is_open] [bit] NULL
CONSTRAINT [PK_Question] PRIMARY KEY CLUSTERED 
(
	[question_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD  CONSTRAINT [FK_Question_Tag] FOREIGN KEY([question_tag])
REFERENCES [dbo].[Tag] ([tag_id])
GO
ALTER TABLE [dbo].[Question] CHECK CONSTRAINT [FK_Question_Tag]
GO
ALTER TABLE [dbo].[Question] ADD CONSTRAINT df_question_vote DEFAULT 0 FOR [question_vote];
GO
--=========================================================================================================================================================
--=========================================================================================================================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT TOP 1 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Answer')
BEGIN
CREATE TABLE [dbo].[Answer](
	[answer_id] [bigint] IDENTITY(1,1) NOT NULL,
	[answer_question] [bigint] NOT NULL,
	[answer_content] [nvarchar](max) NOT NULL,
	[answer_vote] [bigint] NULL,
CONSTRAINT [PK_Answer] PRIMARY KEY CLUSTERED 
(
	[answer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER TABLE [dbo].[Answer]  WITH CHECK ADD  CONSTRAINT [FK_Answer_Question] FOREIGN KEY([answer_question])
REFERENCES [dbo].[Question] ([question_id])
GO
ALTER TABLE [dbo].[Answer] CHECK CONSTRAINT [FK_Answer_Question]
GO
ALTER TABLE [dbo].[Answer] ADD CONSTRAINT df_answer_vote DEFAULT 0 FOR [answer_vote];
GO