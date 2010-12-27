DECLARE @isSuccessful bit

IF '$(IntegrationStatus)' = 'Success' SET @isSuccessful = 1
ELSE SET @isSuccessful = 0

EXEC stp_Build_Add
    @ProjectName = 'BuildPipeline'
    , @SourceUrlBase = 'http://aragorn:81/svn/dev/projects/Uncas.BuildPipeline/'
    , @SourceUrl = '$(SourceUrl)'
    , @SourceRevision = $(SourceRevision)
    , @IsSuccessFul = @isSuccessful
    , @StepName = '$(StepName)'
    , @BuildNumber = $(BuildNumber)