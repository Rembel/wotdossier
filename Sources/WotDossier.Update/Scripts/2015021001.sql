Delete from PlayerAchievements where Id in (select AchievementsId from PlayerStatistic where date(Updated) in (date('2015-02-10'), date('2015-02-11')));
delete from PlayerStatistic where date(Updated) in (date('2015-02-10'), date('2015-02-11'));
delete from TankStatistic where date(Updated) in (date('2015-02-10'), date('2015-02-11'));