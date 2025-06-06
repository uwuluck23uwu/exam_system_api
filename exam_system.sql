USE [EXAM_SYSTEM]
GO
/****** Object:  Table [dbo].[es_multiple_choice_questions]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_multiple_choice_questions](
	[question_id] [int] IDENTITY(1,1) NOT NULL,
	[question_text] [nvarchar](250) NOT NULL,
	[choice_a] [nvarchar](250) NOT NULL,
	[choice_b] [nvarchar](250) NOT NULL,
	[choice_c] [nvarchar](250) NOT NULL,
	[choice_d] [nvarchar](250) NOT NULL,
	[correct_answer] [varchar](50) NOT NULL,
	[std_group] [varchar](10) NOT NULL,
	[std_type] [varchar](10) NOT NULL,
	[ref_std_id] [varchar](15) NOT NULL,
	[difficulty] [varchar](1) NULL,
	[created_date] [datetime] NOT NULL,
	[created_by] [nvarchar](250) NOT NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [nvarchar](250) NULL,
	[is_used] [varchar](1) NOT NULL,
 CONSTRAINT [PK_es_multiple_choice_questions] PRIMARY KEY CLUSTERED 
(
	[question_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[v_es_multiple_choice_questions_eoc]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_es_multiple_choice_questions_eoc] AS SELECT
    q.*,
    eoc.EOC_STD_CODE,
    eoc.EOC_NAME,
    eoc.UOC_STD_CODE,
    eoc.UOC_NAME
FROM
    [EXAM_SYSTEM].[dbo].[es_multiple_choice_questions] q
    LEFT JOIN (
        SELECT
            uoc.UOC_CODE,
            CAST(uoc.UOC_NAME AS NVARCHAR(MAX)) AS UOC_NAME,
            uoc.STD_CODE AS UOC_STD_CODE,
            eoc.EOC_CODE,
            CAST(eoc.EOC_NAME AS NVARCHAR(MAX)) AS EOC_NAME,
            eoc.STD_CODE AS EOC_STD_CODE
        FROM
            [TPQIDG].[dbo].[PSD_UOC] uoc
            LEFT JOIN [TPQIDG].[dbo].[PSD_EOC] eoc 
                ON eoc.UOC_CODE = uoc.UOC_CODE
    ) AS eoc 
    ON eoc.EOC_STD_CODE COLLATE Thai_CI_AI = LEFT(q.ref_std_id, CHARINDEX('-', q.ref_std_id) - 1) COLLATE Thai_CI_AI;
GO
/****** Object:  Table [dbo].[es_exam_schedules]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_exam_schedules](
	[schedule_id] [int] IDENTITY(1,1) NOT NULL,
	[enrollment_id] [int] NOT NULL,
	[part_no] [int] NOT NULL,
	[schedule_date] [date] NOT NULL,
	[start_time] [time](7) NOT NULL,
	[end_time] [time](7) NOT NULL,
	[total_questions] [int] NOT NULL,
	[status] [int] NOT NULL,
	[created_date] [datetime] NOT NULL,
	[updated_date] [datetime] NULL,
 CONSTRAINT [PK_exam_schedules] PRIMARY KEY CLUSTERED 
(
	[schedule_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[es_exam_taken]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_exam_taken](
	[exam_taken_id] [int] IDENTITY(1,1) NOT NULL,
	[schedule_id] [int] NOT NULL,
	[started_time] [time](7) NOT NULL,
	[submitted_time] [time](7) NULL,
	[score] [float] NULL,
	[status] [varchar](1) NULL,
 CONSTRAINT [PK__exam_tak__1EC7A272006A86AA] PRIMARY KEY CLUSTERED 
(
	[exam_taken_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[es_exam_taken_detail]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_exam_taken_detail](
	[exam_taken_detail_id] [int] IDENTITY(1,1) NOT NULL,
	[exam_taken_id] [int] NOT NULL,
	[choice_answer] [text] NOT NULL,
	[num_correct] [int] NOT NULL,
	[num_incorrect] [int] NOT NULL,
	[created_date] [datetime] NULL,
	[created_by] [nvarchar](150) NULL,
PRIMARY KEY CLUSTERED 
(
	[exam_taken_detail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[v_es_exam_taken_eoc]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_es_exam_taken_eoc] AS SELECT
  s.enrollment_id,
  a.schedule_id,
  d.exam_taken_detail_id,
  d.exam_taken_id,
  q.question_id,
  q.question_text,
  answers.Answer AS answer,
   CASE
     WHEN answers.Answer = q.correct_answer THEN 1
     ELSE 0
   END AS score,
  q.std_group,
  q.std_type,
  q.ref_std_id,
  LEFT(q.ref_std_id, CHARINDEX('-', q.ref_std_id) - 1) AS eoc_std_code,
  q.difficulty,
  eoc.UOC_STD_CODE,
  eoc.UOC_NAME
FROM
  [EXAM_SYSTEM].[dbo].[es_exam_taken] a
  LEFT JOIN [EXAM_SYSTEM].[dbo].[es_exam_taken_detail] d ON d.exam_taken_id = a.exam_taken_id
  CROSS APPLY OPENJSON(d.choice_answer)
  WITH (
    Id INT '$.Id',
    Answer NVARCHAR(10) '$.Answer'
  ) AS answers
  LEFT JOIN [EXAM_SYSTEM].[dbo].[es_multiple_choice_questions] q ON q.question_id = answers.Id
  LEFT JOIN [EXAM_SYSTEM].[dbo].[es_exam_schedules] s ON s.schedule_id = a.schedule_id
      LEFT JOIN (
        SELECT
            uoc.UOC_CODE,
            CAST(uoc.UOC_NAME AS NVARCHAR(MAX)) AS UOC_NAME,
            uoc.STD_CODE AS UOC_STD_CODE,
            eoc.EOC_CODE,
            CAST(eoc.EOC_NAME AS NVARCHAR(MAX)) AS EOC_NAME,
            eoc.STD_CODE AS EOC_STD_CODE
        FROM
            [TPQIDG].[dbo].[PSD_UOC] uoc
            LEFT JOIN [TPQIDG].[dbo].[PSD_EOC] eoc 
                ON eoc.UOC_CODE = uoc.UOC_CODE
    ) AS eoc 
    ON eoc.EOC_STD_CODE COLLATE Thai_CI_AI = LEFT(q.ref_std_id, CHARINDEX('-', q.ref_std_id) - 1) COLLATE Thai_CI_AI
GO
/****** Object:  Table [dbo].[es_exam_enrollments]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_exam_enrollments](
	[enrollment_id] [int] IDENTITY(1,1) NOT NULL,
	[num_days] [int] NOT NULL,
	[person_id] [int] NOT NULL,
	[examset_id] [int] NOT NULL,
	[created_date] [datetime] NOT NULL,
	[updated_date] [datetime] NULL,
 CONSTRAINT [PK_es_exam_enrollments] PRIMARY KEY CLUSTERED 
(
	[enrollment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[v_es_exam_taken_uoc_summary]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_es_exam_taken_uoc_summary] AS
SELECT
  e.person_id,
  s.enrollment_id,
  a.schedule_id,
  eoc.UOC_STD_CODE,
  eoc.UOC_NAME,
  COUNT(*) AS question_total,
  SUM(
      CASE 
        WHEN answers.Answer = q.correct_answer THEN 1 
        ELSE 0 
      END
  ) AS score,
  CAST(
    100.0 * SUM(
      CASE 
        WHEN answers.Answer = q.correct_answer THEN 1 
        ELSE 0 
      END
    ) / COUNT(*) AS DECIMAL(5,2)
  ) AS percentage_score
  
FROM
  [EXAM_SYSTEM].[dbo].[es_exam_taken] a
  LEFT JOIN [EXAM_SYSTEM].[dbo].[es_exam_taken_detail] d ON d.exam_taken_id = a.exam_taken_id
  CROSS APPLY OPENJSON(d.choice_answer)
  WITH (
    Id INT '$.Id',
    Answer NVARCHAR(10) '$.Answer'
  ) AS answers
  LEFT JOIN [EXAM_SYSTEM].[dbo].[es_multiple_choice_questions] q ON q.question_id = answers.Id
  LEFT JOIN [EXAM_SYSTEM].[dbo].[es_exam_schedules] s ON s.schedule_id = a.schedule_id
  LEFT JOIN [EXAM_SYSTEM].[dbo].[es_exam_enrollments] e ON e.enrollment_id = s.enrollment_id
  LEFT JOIN (
    SELECT
        uoc.UOC_CODE,
        CAST(uoc.UOC_NAME AS NVARCHAR(MAX)) AS UOC_NAME,
        uoc.STD_CODE AS UOC_STD_CODE,
        eoc.EOC_CODE,
        CAST(eoc.EOC_NAME AS NVARCHAR(MAX)) AS EOC_NAME,
        eoc.STD_CODE AS EOC_STD_CODE
    FROM
        [TPQIDG].[dbo].[PSD_UOC] uoc
        LEFT JOIN [TPQIDG].[dbo].[PSD_EOC] eoc 
            ON eoc.UOC_CODE = uoc.UOC_CODE
  ) AS eoc 
  ON eoc.EOC_STD_CODE COLLATE Thai_CI_AI = LEFT(q.ref_std_id, CHARINDEX('-', q.ref_std_id) - 1) COLLATE Thai_CI_AI

GROUP BY 
  e.person_id,
  s.enrollment_id, 
  a.schedule_id,  
  eoc.UOC_STD_CODE, 
  eoc.UOC_NAME
GO
/****** Object:  Table [dbo].[es_exam_sets]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_exam_sets](
	[examset_id] [int] IDENTITY(1,1) NOT NULL,
	[examset_type] [int] NOT NULL,
	[examset_name] [varchar](100) NOT NULL,
	[std_id] [int] NOT NULL,
	[std_name] [nvarchar](250) NOT NULL,
	[description] [text] NULL,
	[total_questions] [int] NOT NULL,
	[max_score] [int] NOT NULL,
	[pass_percentage] [decimal](5, 2) NOT NULL,
	[num_date_exam] [int] NOT NULL,
	[start_date] [date] NOT NULL,
	[end_date] [date] NOT NULL,
	[time_per_question] [int] NOT NULL,
	[created_by] [nvarchar](150) NOT NULL,
	[updated_by] [nvarchar](150) NULL,
	[created_date] [datetime] NOT NULL,
	[updated_date] [datetime] NULL,
	[is_used] [nchar](1) NOT NULL,
 CONSTRAINT [PK__exam_set__EA6772E4F5B0C197] PRIMARY KEY CLUSTERED 
(
	[examset_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[v_es_exam_schedule_details]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[v_es_exam_schedule_details] AS
SELECT
  s.*,
  em.num_days,
  em.person_id,
  es.examset_name,
  es.std_name,
  es.start_date,
  es.end_date,
  es.max_score,
  es.num_date_exam,
  es.examset_type,
  es.pass_percentage,
  et.exam_taken_id,
  et.started_time,
  et.submitted_time,
  et.status AS exam_taken_status,
  q.score,
  q.total_score,
  q.percentage_score
FROM
  [EXAM_SYSTEM].[dbo].[es_exam_schedules] s
  LEFT JOIN [EXAM_SYSTEM].[dbo].[es_exam_enrollments] em ON em.enrollment_id = s.enrollment_id
  LEFT JOIN [EXAM_SYSTEM].[dbo].[es_exam_sets] es ON es.examset_id = em.examset_id
  LEFT JOIN [EXAM_SYSTEM].[dbo].[es_exam_taken] et ON et.schedule_id = s.schedule_id
  LEFT JOIN (
    SELECT
      schedule_id,
      SUM(score) AS score,
      COUNT(*) AS total_score,
      CAST(ROUND(100.0 * SUM(score) / COUNT(*), 0) AS INT) AS percentage_score 
    FROM
      v_es_exam_taken_eoc 
    GROUP BY
      schedule_id
  ) q ON q.schedule_id = s.schedule_id;
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[activity_log]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[activity_log](
	[log_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[action] [varchar](255) NOT NULL,
	[timestamp] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[log_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[es_choices]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_choices](
	[choice_id] [int] IDENTITY(1,1) NOT NULL,
	[question_id] [int] NOT NULL,
	[choice_text] [text] NOT NULL,
	[is_correct] [bit] NOT NULL,
 CONSTRAINT [PK__choices__33CAF83AF7B0737B] PRIMARY KEY CLUSTERED 
(
	[choice_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[es_exam_final_results]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_exam_final_results](
	[result_id] [int] IDENTITY(1,1) NOT NULL,
	[person_id] [int] NOT NULL,
	[examset_id] [int] NOT NULL,
	[total_score] [float] NULL,
	[total_days] [int] NOT NULL,
	[status] [varchar](1) NULL,
	[completed_date] [timestamp] NULL,
 CONSTRAINT [PK__exam_fin__AFB3C3164D8DA42D] PRIMARY KEY CLUSTERED 
(
	[result_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[es_exam_instructions]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_exam_instructions](
	[instruction_id] [int] IDENTITY(1,1) NOT NULL,
	[examset_id] [int] NOT NULL,
	[title] [nvarchar](150) NOT NULL,
	[description] [text] NULL,
	[show_before_exam] [bit] NOT NULL,
	[created_date] [datetime] NOT NULL,
	[created_by] [nvarchar](150) NOT NULL,
	[updated_date] [datetime] NULL,
	[updated_by] [nvarchar](150) NULL,
 CONSTRAINT [PK_es_exam_instructions] PRIMARY KEY CLUSTERED 
(
	[instruction_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[es_exam_user_selection]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_exam_user_selection](
	[selection_id] [int] IDENTITY(1,1) NOT NULL,
	[person_id] [int] NOT NULL,
	[examset_id] [int] NOT NULL,
	[status] [varchar](1) NULL,
	[total_days] [int] NOT NULL,
	[created_date] [datetime] NULL,
	[completed_at] [datetime] NULL,
 CONSTRAINT [PK__exam_use__010BE539D8B988E3] PRIMARY KEY CLUSTERED 
(
	[selection_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[es_gcio_candidates]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_gcio_candidates](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[initial_name] [nvarchar](50) NOT NULL,
	[first_name] [nvarchar](150) NOT NULL,
	[last_name] [nvarchar](150) NOT NULL,
	[org_name] [nvarchar](250) NULL,
	[position_name] [nvarchar](150) NULL,
	[email] [nvarchar](50) NULL,
	[mobile_phone] [nvarchar](10) NULL,
	[remark] [text] NULL,
	[is_used] [bit] NOT NULL,
 CONSTRAINT [PK_es_gcio_candidates] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[es_generated_exam_parts]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_generated_exam_parts](
	[generated_part_id] [int] IDENTITY(1,1) NOT NULL,
	[selection_id] [int] NOT NULL,
	[part_number] [int] NOT NULL,
	[num_questions] [int] NOT NULL,
	[scheduled_date] [date] NOT NULL,
	[created_date] [datetime] NOT NULL,
	[person_id] [int] NOT NULL,
 CONSTRAINT [PK__generate__31E71B922A2D0DD1] PRIMARY KEY CLUSTERED 
(
	[generated_part_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[es_generated_exam_questions]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_generated_exam_questions](
	[generated_question_id] [int] IDENTITY(1,1) NOT NULL,
	[schedule_id] [int] NOT NULL,
	[question_id] [int] NOT NULL,
 CONSTRAINT [PK__generate__5741C38A6543A9B1] PRIMARY KEY CLUSTERED 
(
	[generated_question_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[es_questions]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[es_questions](
	[question_id] [int] IDENTITY(1,1) NOT NULL,
	[question_text] [text] NOT NULL,
	[question_type] [varchar](1) NOT NULL,
	[std_group] [varchar](3) NULL,
	[std_type] [varchar](5) NULL,
	[ref_std_id] [varchar](15) NULL,
	[difficulty] [varchar](1) NOT NULL,
	[subject] [varchar](100) NOT NULL,
	[created_by] [nvarchar](150) NOT NULL,
	[approved_by] [nvarchar](150) NULL,
	[created_date] [datetime] NULL,
	[approved_at] [datetime] NULL,
 CONSTRAINT [PK__question__2EC21549CB6E8A7F] PRIMARY KEY CLUSTERED 
(
	[question_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 27/4/2568 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NOT NULL,
	[password_hash] [varchar](255) NOT NULL,
	[full_name] [varchar](100) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [UQ__users__AB6E6164810BD010] UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__users__F3DBC572BC65037F] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[es_exam_final_results] ADD  CONSTRAINT [DF__exam_fina__total__4AB81AF0]  DEFAULT ((0)) FOR [total_score]
GO
ALTER TABLE [dbo].[es_exam_schedules] ADD  CONSTRAINT [DF_es_exam_schedules_status]  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [dbo].[es_exam_sets] ADD  CONSTRAINT [DF__exam_sets__total__398D8EEE]  DEFAULT ((300)) FOR [total_questions]
GO
ALTER TABLE [dbo].[es_exam_sets] ADD  CONSTRAINT [DF_es_exam_sets_is_used1]  DEFAULT ((1)) FOR [is_used]
GO
ALTER TABLE [dbo].[es_exam_taken] ADD  CONSTRAINT [DF__exam_take__score__47DBAE45]  DEFAULT (NULL) FOR [score]
GO
ALTER TABLE [dbo].[es_multiple_choice_questions] ADD  CONSTRAINT [DF_es_multiple_choice_questions_is_used]  DEFAULT ((1)) FOR [is_used]
GO
ALTER TABLE [dbo].[es_questions] ADD  CONSTRAINT [DF__questions__appro__3C69FB99]  DEFAULT (NULL) FOR [approved_by]
GO
ALTER TABLE [dbo].[es_exam_user_selection]  WITH CHECK ADD  CONSTRAINT [CK__exam_user__total__412EB0B6] CHECK  (([total_days]>=(1) AND [total_days]<=(5)))
GO
ALTER TABLE [dbo].[es_exam_user_selection] CHECK CONSTRAINT [CK__exam_user__total__412EB0B6]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสตัวเลือก' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_choices', @level2type=N'COLUMN',@level2name=N'choice_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสคำถามที่ตัวเลือกนี้เป็นของมัน' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_choices', @level2type=N'COLUMN',@level2name=N'question_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'เนื้อหาของตัวเลือก' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_choices', @level2type=N'COLUMN',@level2name=N'choice_text'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ระบุว่าตัวเลือกนี้เป็นคำตอบที่ถูกต้องหรือไม่ (true/false)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_choices', @level2type=N'COLUMN',@level2name=N'is_correct'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสการลงทะเบียนสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_enrollments', @level2type=N'COLUMN',@level2name=N'enrollment_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'จำนวนที่ระบุในการสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_enrollments', @level2type=N'COLUMN',@level2name=N'num_days'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสบุคคล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_enrollments', @level2type=N'COLUMN',@level2name=N'person_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสชุดข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_enrollments', @level2type=N'COLUMN',@level2name=N'examset_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสผลสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_final_results', @level2type=N'COLUMN',@level2name=N'result_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสนักเรียน' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_final_results', @level2type=N'COLUMN',@level2name=N'person_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสชุดข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_final_results', @level2type=N'COLUMN',@level2name=N'examset_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'คะแนนรวมจากทุก Part' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_final_results', @level2type=N'COLUMN',@level2name=N'total_score'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'จำนวนวันที่สอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_final_results', @level2type=N'COLUMN',@level2name=N'total_days'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'สถานะสอบเสร็จหรือยัง' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_final_results', @level2type=N'COLUMN',@level2name=N'status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่รวมคะแนนเสร็จ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_final_results', @level2type=N'COLUMN',@level2name=N'completed_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสคำชี้แจง' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_instructions', @level2type=N'COLUMN',@level2name=N'instruction_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสชุดข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_instructions', @level2type=N'COLUMN',@level2name=N'examset_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่สร้างรายการข้อมูล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_instructions', @level2type=N'COLUMN',@level2name=N'created_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ผู้สร้างรายการข้อมูล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_instructions', @level2type=N'COLUMN',@level2name=N'created_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่ปรับปรุงรายการข้อมูล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_instructions', @level2type=N'COLUMN',@level2name=N'updated_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ผู้ปรับปรุงรายการข้อมูล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_instructions', @level2type=N'COLUMN',@level2name=N'updated_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสการจัดตารางสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_schedules', @level2type=N'COLUMN',@level2name=N'schedule_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสบุคคล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_schedules', @level2type=N'COLUMN',@level2name=N'enrollment_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ครั้งที่สอบ เช่น ครั้งที่ 1, 2, 3 เป็นต้น' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_schedules', @level2type=N'COLUMN',@level2name=N'part_no'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่สอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_schedules', @level2type=N'COLUMN',@level2name=N'schedule_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'เวลาเริ่มสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_schedules', @level2type=N'COLUMN',@level2name=N'start_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'เวลาสิ้นสุดการสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_schedules', @level2type=N'COLUMN',@level2name=N'end_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'จำนวนข้อ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_schedules', @level2type=N'COLUMN',@level2name=N'total_questions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'สถานะ 0 คือ ยังไม่ได้สอบ , 1 คือ สอบแล้ว, 2 คือ ไม่ได้สอบ (ยกเลิการสอบอัตโนมัติ)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_schedules', @level2type=N'COLUMN',@level2name=N'status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่สร้างรายการ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_schedules', @level2type=N'COLUMN',@level2name=N'created_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่ปรับปรุงรายการ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_schedules', @level2type=N'COLUMN',@level2name=N'updated_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสชุดข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'examset_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ประเภทชุดข้อสอบ เช่น 1 คือ GCIO, 2 คือ มาตรฐาน DG' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'examset_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ชื่อชุดข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'examset_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสมาตรฐาน' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'std_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ชื่อมาตรฐาน' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'std_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รายละเอียดของชุดข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'description'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'จำนวนข้อสอบทั้งหมด' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'total_questions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'คะแนนเต็มของข้อสอบชุดนี้' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'max_score'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'เปอร์เซ็นต์ที่ต้องได้เพื่อผ่าน เช่น 60.00' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'pass_percentage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'จำนวนวันที่ใช้สอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'num_date_exam'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่เริ่มให้มีการสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'start_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่สิ้นสุดของการสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'end_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ระยะเวลาทำข้อสอบต่อ 1 ข้อ  (หน่วย: วินาที)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'time_per_question'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ผู้สร้างชุดข้อสอบ (เชื่อมกับ Users)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'created_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่สร้าง' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_sets', @level2type=N'COLUMN',@level2name=N'created_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสของการสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_taken', @level2type=N'COLUMN',@level2name=N'exam_taken_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสของ Part ที่กำลังสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_taken', @level2type=N'COLUMN',@level2name=N'schedule_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'เวลาที่เริ่มสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_taken', @level2type=N'COLUMN',@level2name=N'started_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'เวลาที่ส่งข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_taken', @level2type=N'COLUMN',@level2name=N'submitted_time'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'สถานะการสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_taken', @level2type=N'COLUMN',@level2name=N'status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสการทำแบบทดสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_taken_detail', @level2type=N'COLUMN',@level2name=N'exam_taken_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'จำนวนข้อที่ตอบถูก' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_taken_detail', @level2type=N'COLUMN',@level2name=N'num_correct'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'จำนวนข้อที่ตอบผิด' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_taken_detail', @level2type=N'COLUMN',@level2name=N'num_incorrect'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสของรายการเลือก' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_user_selection', @level2type=N'COLUMN',@level2name=N'selection_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสผู้สอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_user_selection', @level2type=N'COLUMN',@level2name=N'person_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสชุดข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_user_selection', @level2type=N'COLUMN',@level2name=N'examset_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'สถานะการสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_user_selection', @level2type=N'COLUMN',@level2name=N'status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'จำนวนวันสอบที่เลือก (1-5 วัน)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_user_selection', @level2type=N'COLUMN',@level2name=N'total_days'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่สร้าง' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_user_selection', @level2type=N'COLUMN',@level2name=N'created_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่สอบเสร็จ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_exam_user_selection', @level2type=N'COLUMN',@level2name=N'completed_at'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสออโต้รัน' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_gcio_candidates', @level2type=N'COLUMN',@level2name=N'id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'คำนำหน้าชื่อ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_gcio_candidates', @level2type=N'COLUMN',@level2name=N'initial_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ชื่อ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_gcio_candidates', @level2type=N'COLUMN',@level2name=N'first_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'นามสกุล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_gcio_candidates', @level2type=N'COLUMN',@level2name=N'last_name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'อีเมล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_gcio_candidates', @level2type=N'COLUMN',@level2name=N'email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'เบอร์โทรศัพท์' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_gcio_candidates', @level2type=N'COLUMN',@level2name=N'mobile_phone'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'หมายเหตุ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_gcio_candidates', @level2type=N'COLUMN',@level2name=N'remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'สถานะการใช้งาน 1 คือ ใช้งาน, 0 คือ ไม่ใช้งาน' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_gcio_candidates', @level2type=N'COLUMN',@level2name=N'is_used'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสของ Part' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_generated_exam_parts', @level2type=N'COLUMN',@level2name=N'generated_part_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสของรายการเลือก' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_generated_exam_parts', @level2type=N'COLUMN',@level2name=N'selection_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่ของการสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_generated_exam_parts', @level2type=N'COLUMN',@level2name=N'part_number'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'จำนวนข้อสอบใน Part นี้' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_generated_exam_parts', @level2type=N'COLUMN',@level2name=N'num_questions'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่กำหนดสอบของ Part นี้' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_generated_exam_parts', @level2type=N'COLUMN',@level2name=N'scheduled_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่สร้าง' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_generated_exam_parts', @level2type=N'COLUMN',@level2name=N'created_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสบุคคล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_generated_exam_parts', @level2type=N'COLUMN',@level2name=N'person_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสรายการ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_generated_exam_questions', @level2type=N'COLUMN',@level2name=N'generated_question_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสของ Part' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_generated_exam_questions', @level2type=N'COLUMN',@level2name=N'schedule_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ข้อสอบที่ถูกสุ่ม' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_generated_exam_questions', @level2type=N'COLUMN',@level2name=N'question_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสคำถาม' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'question_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'คำถาม' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'question_text'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ตัวเลือก ก' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'choice_a'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ตัวเลือก ข' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'choice_b'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ตัวเลือก ค' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'choice_c'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ตัวเลือก ง' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'choice_d'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'คำตอบที่ถูกต้อง' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'correct_answer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสกลุ่มมาตรฐาน' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'std_group'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ออกข้อสอบตามประเภทใด ( 1คือ คุณวุฒิ, 2 คือ UOC, 3 คือ EOC,  4 คือ PC)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'std_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสอ้างอิงของมาตรฐาน' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'ref_std_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ระดับความยากง่าย' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'difficulty'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่สร้างรายการข้อมูล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'created_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ผู้สร้างรายการข้อมูล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'created_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่ปรับปรุงรายการข้อมูล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'updated_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ผู้ปรับปรุงรายการข้อมูล' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'updated_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'สถานะการใช้งาน 1 คือ ใช้งาน , 0 คือ ไม่ใช้งาน' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_multiple_choice_questions', @level2type=N'COLUMN',@level2name=N'is_used'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'รหัสข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_questions', @level2type=N'COLUMN',@level2name=N'question_id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'เนื้อหาของคำถาม' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_questions', @level2type=N'COLUMN',@level2name=N'question_text'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ประเภทของคำถาม' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_questions', @level2type=N'COLUMN',@level2name=N'question_type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'กลุ่มมาตรฐาน' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_questions', @level2type=N'COLUMN',@level2name=N'std_group'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ระดับความยากของข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_questions', @level2type=N'COLUMN',@level2name=N'difficulty'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วิชาของข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_questions', @level2type=N'COLUMN',@level2name=N'subject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ผู้สร้างข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_questions', @level2type=N'COLUMN',@level2name=N'created_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ผู้อนุมัติข้อสอบ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_questions', @level2type=N'COLUMN',@level2name=N'approved_by'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่สร้าง' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_questions', @level2type=N'COLUMN',@level2name=N'created_date'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'วันที่อนุมัติ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'es_questions', @level2type=N'COLUMN',@level2name=N'approved_at'
GO
