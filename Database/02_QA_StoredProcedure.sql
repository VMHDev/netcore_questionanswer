--=========================================================================================================================================================
--=========================================================================================================================================================
IF EXISTS (SELECT TOP 1 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proTag_GetAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[proTag_GetAll]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[proTag_GetAll]
AS
	SELECT	[tag_id],
			[tag_name]
	FROM [Tag]
GO