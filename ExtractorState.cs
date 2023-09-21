namespace DMIExtractorV2;

public enum ExtractorState
{
    UNINITIALIZED,
    FILEPATHS_SET,
    CHECKING,
    READY_TO_EXTRACT,
    EXTRACTING,
    DONE
}
