ALTER PROCEDURE stp_Build_Add
(
    @ProjectName nvarchar(64)
    , @SourceUrlBase nvarchar(256)
    , @SourceUrl nvarchar(256)
    , @SourceRevision int
    , @IsSuccessful bit
    , @StepName nvarchar(64)
    , @BuildNumber int
)
AS
BEGIN
    DECLARE @projectId int
    SELECT @projectId = ProjectId FROM Project WHERE ProjectName = @ProjectName
    IF @projectId IS NULL
    BEGIN
        INSERT INTO Project (ProjectName, SourceUrlBase) VALUES (@ProjectName, @SourceUrlBase)
        
        SET @projectId = @@IDENTITY
    END
    
    DECLARE @pipelineId int
    SELECT @pipelineId = PipelineId FROM Pipeline WHERE ProjectId = @projectId
        AND SourceRevision = @SourceRevision AND SourceUrl = @SourceUrl
    IF @pipelineId IS NULL
    BEGIN
        INSERT INTO Pipeline (ProjectId, SourceUrl, SourceRevision)
        VALUES (@projectId, @SourceUrl, @SourceRevision)
        
        SET @pipelineId = @@IDENTITY
    END
    
    INSERT INTO BuildStep (PipelineId, BuildNumber, IsSuccessful, StepName)
    VALUES (@pipelineId, @BuildNumber, @IsSuccessful, @StepName)
END