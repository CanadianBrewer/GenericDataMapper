CREATE TABLE [dbo].[Person]
(
    [id]            [int] IDENTITY (1,1) NOT NULL,
    [first_name]    [varchar](50)        NOT NULL,
    [last_name]     [varchar](50)        NOT NULL,
    [city]          [varchar](100)       NULL,
    [date_of_birth] [datetime]           NULL,
    [is_employee]   [bit]                NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Company]
(
    [id]                  [int] IDENTITY (1,1) NOT NULL,
    [corporate_name]      [varchar](50)        NOT NULL,
    [ticker_symbol]       [varchar](50)        NULL,
    [current_stock_price] [decimal](18, 8)     NULL,
    [fiscal_year_end]     [datetime]           NULL,
    [board_member_1]      [varchar](100)       NULL,
    [board_member_2]      [varchar](100)       NULL,
    [board_member_3]      [varchar](100)       NULL
) ON [PRIMARY]
GO