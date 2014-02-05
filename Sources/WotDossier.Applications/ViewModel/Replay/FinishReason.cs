namespace WotDossier.Applications.ViewModel.Replay
{
    public enum FinishReason
    {
        Unknown = 0,        //Unknown (not used)
        Extermination = 1, 	//Extermination, all enemy vehicles were destroyed
        BaseCapture = 2,    //Base capture, the enemy base was captured
        Timeout = 3,        //Timeout, the battle time has elapsed
        Failure = 4,         //Failure, the arena did not initialize or had an internal failure
        Technical = 5         //Technical, the match was interrupted by a server restart or was otherwise cancelled by the server 
    }
}