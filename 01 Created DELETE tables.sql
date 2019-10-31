USE [CAS]
GO
/****** Object:  Table [dbo].[Customer_Condition_DELETED]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer_Condition_DELETED](
	[Customer_Condition_ID] [int] NOT NULL,
	[Customer_ID] [int] NULL,
	[KYC_Check_Status] [varchar](255) NULL,
	[Date_KYC_Checks_Completed] [datetime] NULL,
	[Condition] [varchar](255) NULL,
	[Date_Conditions_Satisfied] [datetime] NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer_DELETED]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer_DELETED](
	[Customer_ID] [int] NOT NULL,
	[Group_ID] [int] NULL,
	[Customer_Name] [varchar](255) NULL,
	[CIF_Number] [varchar](255) NULL,
	[Account_No] [varchar](255) NULL,
	[Type_of_Entity] [varchar](255) NULL,
	[Date_of_Relationship] [datetime] NULL,
	[Country_of_Domicile] [varchar](255) NULL,
	[Country_of_Risk] [varchar](255) NULL,
	[Special_Category_Connected_Accounts] [varchar](255) NULL,
	[Industry_SIC_Code] [varchar](255) NULL,
	[Relationship_Officer] [varchar](255) NULL,
	[NBAD_Internal_Rating_Current] [varchar](255) NULL,
	[NBAD_Internal_Rating_Proposed] [varchar](255) NULL,
	[Rating_Model_Used] [varchar](255) NULL,
	[Last_Annual_Review_Date] [datetime] NULL,
	[Next_Annual_Review_Date] [datetime] NULL,
	[Activity] [varchar](255) NULL,
	[Date_of_Establishment] [datetime] NULL,
	[Account_Opening_Date] [datetime] NULL,
	[Industry_description] [varchar](255) NULL,
	[Asset_Class] [varchar](255) NULL,
	[Interim_Review_Date] [datetime] NULL,
	[Brief_History__Background] [varchar](255) NULL,
	[Last_Call_Date] [datetime] NULL,
	[Last_Call_Notes] [varchar](255) NULL,
	[Customer_Status] [varchar](255) NULL,
	[Customer_Notes] [varchar](max) NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer_Library_DELETED]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer_Library_DELETED](
	[File_Id] [int] NOT NULL,
	[Customer_ID] [int] NULL,
	[File_Name] [varchar](255) NULL,
	[File_Description] [varchar](255) NULL,
	[File] [varbinary](max) NULL,
	[Date_Added] [datetime] NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Document_Condition_DELETED]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document_Condition_DELETED](
	[Document_Condition_ID] [int] NOT NULL,
	[Document_ID] [int] NULL,
	[Condition_Type] [varchar](255) NULL,
	[Condition_Name] [varchar](255) NULL,
	[Date_Satisfied] [datetime] NULL,
	[Date_of_Next_Condition_Check] [datetime] NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Document_DELETED]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document_DELETED](
	[Document_ID] [int] NOT NULL,
	[Type_Of_Document] [varchar](255) NULL,
	[Documentation_Description] [varchar](255) NULL,
	[Documentation_Status] [varchar](255) NULL,
	[Date_Drafted_and_Signed_Internally] [datetime] NULL,
	[Internal_Confirmation_That_Terms_Match_Sanction] [varchar](255) NULL,
	[Date_Sent_To_Customer] [datetime] NULL,
	[Date_Executed] [datetime] NULL,
	[Confirmation_That_Signatures_Match_Mandate] [varchar](255) NULL,
	[Expiry__Review_Date_of_Document] [datetime] NULL,
	[Date_To_Safe] [datetime] NULL,
	[Customer_ID] [int] NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Document_X_Security_DELETED]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document_X_Security_DELETED](
	[Doc_X_Sec_ID] [int] NOT NULL,
	[Document_ID] [int] NULL,
	[Security_ID] [int] NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Facility_Condition_DELETED]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Facility_Condition_DELETED](
	[Facility_Condition_ID] [int] NOT NULL,
	[Facility_ID] [int] NULL,
	[Condition_Name] [varchar](255) NULL,
	[Condition_Type] [varchar](255) NULL,
	[Last_Review_Date] [datetime] NULL,
	[Next_Review_Date] [datetime] NULL,
	[Test_Value] [decimal](18, 2) NULL,
	[Actual_Value] [decimal](18, 2) NULL,
	[Facility_Status] [varchar](255) NULL,
	[Last_Formal_Valuation_Date] [datetime] NULL,
	[Last_Estimated_Valuation_Date] [datetime] NULL,
	[Last_Estimated_LTV] [decimal](18, 2) NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Facility_DELETED]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Facility_DELETED](
	[Facility_ID] [int] NOT NULL,
	[Customer_ID] [int] NULL,
	[Type_of_Facility] [varchar](255) NULL,
	[Facility_Description] [varchar](255) NULL,
	[Seniority] [varchar](255) NULL,
	[Facility_Rating] [varchar](255) NULL,
	[Currency] [varchar](255) NULL,
	[Current_Limit] [decimal](18, 2) NULL,
	[Proposed_Limit] [decimal](18, 2) NULL,
	[Tenor__Final_Maturity] [varchar](255) NULL,
	[Pricing_Base_Rate] [varchar](255) NULL,
	[Pricing_Margin] [decimal](18, 2) NULL,
	[Fees_Payable] [varchar](255) NULL,
	[Date_of_Approval] [datetime] NULL,
	[Facility_Code] [varchar](255) NULL,
	[Funded_Outstanding] [decimal](18, 2) NULL,
	[Unfunded_Outstanding] [decimal](18, 2) NULL,
	[Coverage_Percent] [decimal](18, 2) NULL,
	[LTV] [decimal](18, 2) NULL,
	[Repayment_Schedule] [varchar](255) NULL,
	[Interest__Fee_Payment_Schedule] [varchar](255) NULL,
	[Account_Closure_Date] [datetime] NULL,
	[RAROC] [varchar](255) NULL,
	[Purpose] [varchar](255) NULL,
	[Agent_Name] [varchar](255) NULL,
	[Guarantor] [varchar](255) NULL,
	[Loan_Number] [varchar](255) NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Facility_X_Document_DELETED]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Facility_X_Document_DELETED](
	[Fac_X_Doc_ID] [int] NOT NULL,
	[Facility_ID] [int] NULL,
	[Document_ID] [int] NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Group_DELETED]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group_DELETED](
	[Group_ID] [int] NOT NULL,
	[Group_Name] [varchar](255) NULL,
	[Group_Account_Manager] [varchar](255) NULL,
	[Group_CIF] [varchar](255) NULL,
	[Group_Status] [varchar](255) NULL,
	[Group_Notes] [varchar](max) NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Security_Condition_DELETED]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Security_Condition_DELETED](
	[Security_Condition_ID] [int] NOT NULL,
	[Security_ID] [int] NULL,
	[Security_Condition] [varchar](255) NULL,
	[Date_Conditions_Satisfied] [datetime] NULL,
	[Valuation_Date] [datetime] NULL,
	[Perfection_Status] [varchar](255) NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Security_DELETED ]    Script Date: 29/10/2019 15:58:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Security_DELETED ](
	[Security_ID] [int] NOT NULL,
	[Security_Description] [varchar](255) NULL,
	[Permissible_Collateral__Haircuts_Under_Traded_Product_Collateral_Agreement] [varchar](255) NULL,
	[Date_of_Collateral_Perfection] [datetime] NULL,
	[Location_of_Security__Collateral] [varchar](255) NULL,
	[Original_Valuation_Currency] [varchar](25) NULL,
	[Original_Valuation] [decimal](18, 2) NULL,
	[Basis_for_Original_Valuation] [varchar](255) NULL,
	[Date_of_Last_Valuation] [datetime] NULL,
	[Index_Basis_Used_For_Revaluation] [varchar](255) NULL,
	[Current_Estimate_Value_Currency] [varchar](25) NULL,
	[Current_Estimate_Value] [decimal](18, 2) NULL,
	[Date_of_Next_Valuation] [datetime] NULL,
	[Customer_ID] [int] NULL,
	[Deleted_On] [datetime] NOT NULL
) ON [PRIMARY]
GO
