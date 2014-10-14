namespace WotDossier.Applications.ViewModel.Replay
{
    public enum DeathReason
    {
        Alive = -1, 	            //Alive
        DestroyedByShot = 0, 	    //Destroyed by a shot
        DestroyedByFire = 1, 	    //Destroyed by fire
        DestroyedByRamming = 2, 	//Destroyed by ramming
        Crashed = 3, 	            //Crashed
        DestroyedByDeathZone = 4, 	//Destroyed by a death zone
        VehicleDrowned = 5,         //Vehicle drowned 
    }
}