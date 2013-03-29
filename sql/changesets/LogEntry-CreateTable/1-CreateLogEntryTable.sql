CREATE TABLE LogEntry
(
    Id  bigint  NOT NULL  IDENTITY(1,1)
        CONSTRAINT PK_LogEntry PRIMARY KEY CLUSTERED
    , LogTime  datetime  NOT NULL
        CONSTRAINT DF_LogEntry_LogTime DEFAULT GETDATE()
    , ServiceId  smallint  NOT NULL
    , Version  nvarchar(50)  NOT NULL
    , LogType  tinyint  NOT NULL
    , Description  nvarchar(max)  NOT NULL
    , FileName  nvarchar(500)  NULL
    , LineNumber  int  NULL
    , SimpleStackTrace  nvarchar(max)  NULL
    , StackTrace  nvarchar(max)  NULL
    , ExceptionType  nvarchar(500)  NULL
    , ExceptionMessage  nvarchar(max)  NULL
    , FullException  nvarchar(max)  NULL
)
