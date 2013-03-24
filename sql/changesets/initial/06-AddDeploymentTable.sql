CREATE TABLE Deployment
(
    DeploymentId    int    NOT NULL
        IDENTITY(1, 1)
        CONSTRAINT PK_Deployment PRIMARY KEY CLUSTERED
    , Created    datetime    NOT NULL
        CONSTRAINT DF_Deployment_Created DEFAULT GETDATE()
    , PipelineId    int    NOT NULL
        CONSTRAINT FK_Deployment_Pipeline FOREIGN KEY
            REFERENCES Pipeline (PipelineId)
    , EnvironmentId    int    NOT NULL
    , Started    datetime    NULL
    , Completed    datetime    NULL
)