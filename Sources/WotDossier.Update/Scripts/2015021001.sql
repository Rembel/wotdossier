Delete from PlayerAchievements where Id in (select AchievementsId from PlayerStatistic where date(Updated) >= date('2015-02-10') AND date(Updated) <= CURRENT_DATE);
delete from PlayerStatistic where date(Updated) >= date('2015-02-10') AND date(Updated) <= CURRENT_DATE;
delete from TankStatistic where date(Updated) >= date('2015-02-10') AND date(Updated) <= CURRENT_DATE;