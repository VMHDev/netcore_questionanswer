USE [QA]
GO
SET IDENTITY_INSERT [dbo].[Tag] ON 

INSERT [dbo].[Tag] ([tag_id], [tag_name]) VALUES (1, N'C#')
INSERT [dbo].[Tag] ([tag_id], [tag_name]) VALUES (2, N'VueJS')
INSERT [dbo].[Tag] ([tag_id], [tag_name]) VALUES (3, N'Python')
INSERT [dbo].[Tag] ([tag_id], [tag_name]) VALUES (4, N'Java')
INSERT [dbo].[Tag] ([tag_id], [tag_name]) VALUES (5, N'C/C++')
SET IDENTITY_INSERT [dbo].[Tag] OFF
GO
SET IDENTITY_INSERT [dbo].[Question] ON 

INSERT [dbo].[Question] ([question_id], [question_content], [question_vote], [question_tag], [is_open]) VALUES (1, N'What is C#?', 0, 1, NULL)
INSERT [dbo].[Question] ([question_id], [question_content], [question_vote], [question_tag], [is_open]) VALUES (2, N'What is VueJS?', 0, 2, NULL)
INSERT [dbo].[Question] ([question_id], [question_content], [question_vote], [question_tag], [is_open]) VALUES (3, N'What is Python?', 0, 3, NULL)
INSERT [dbo].[Question] ([question_id], [question_content], [question_vote], [question_tag], [is_open]) VALUES (4, N'What is Java', 0, 4, NULL)
INSERT [dbo].[Question] ([question_id], [question_content], [question_vote], [question_tag], [is_open]) VALUES (5, N'What is C/C++', 0, 5, NULL)
SET IDENTITY_INSERT [dbo].[Question] OFF
GO
SET IDENTITY_INSERT [dbo].[Answer] ON 

INSERT [dbo].[Answer] ([answer_id], [answer_question], [answer_content], [answer_vote]) VALUES (1, 1, N'C# is ...', 0)
INSERT [dbo].[Answer] ([answer_id], [answer_question], [answer_content], [answer_vote]) VALUES (2, 2, N'VueJS is ...', 0)
INSERT [dbo].[Answer] ([answer_id], [answer_question], [answer_content], [answer_vote]) VALUES (3, 3, N'Python is ...', 0)
INSERT [dbo].[Answer] ([answer_id], [answer_question], [answer_content], [answer_vote]) VALUES (4, 4, N'Java is ...', 0)
INSERT [dbo].[Answer] ([answer_id], [answer_question], [answer_content], [answer_vote]) VALUES (5, 5, N'C/C++ is ...', 0)
SET IDENTITY_INSERT [dbo].[Answer] OFF
GO
