# 2013.06.07 15:33:02 Kaliningrad Standard Time
import cPickle
import struct
import json
import time
import sys
import os
from itertools import izip
ARENA_GAMEPLAY_NAMES = ('ctf',
 'domination',
 'assault',
 'escort',
 'ctf2',
 'domination2',
 'assault2')
BONUS_TYPE_NAMES = ('public',
 'training',
 'companywar',
 'tournament',
 'clanwar')
FINISH_REASON_NAMES = ('extermination',
 'base',
 'timeout',
 'failure',
 'technical')
ACHIEVEMENTS = ('_version',
 'creationTime',
 'maxXPVehicle',
 'maxFragsVehicle',
 'lastBattleTime',
 'battleLifeTime',
 'maxFrags',
 'xp',
 'maxXP',
 'battlesCount',
 'wins',
 'losses',
 'survivedBattles',
 'winAndSurvived',
 'frags',
 'frags8p',
 'fragsBeast',
 'shots',
 'hits',
 'spotted',
 'damageDealt',
 'damageReceived',
 'treesCut',
 'capturePoints',
 'droppedCapturePoints',
 'sniperSeries',
 'maxSniperSeries',
 'invincibleSeries',
 'maxInvincibleSeries',
 'diehardSeries',
 'maxDiehardSeries',
 'killingSeries',
 'fragsSinai',
 'maxKillingSeries',
 'piercingSeries',
 'maxPiercingSeries',
 'battleHeroes',
 'warrior',
 'invader',
 'sniper',
 'defender',
 'steelwall',
 'supporter',
 'scout',
 'evileye',
 'medalKay',
 'medalCarius',
 'medalKnispel',
 'medalPoppel',
 'medalAbrams',
 'medalLeClerc',
 'medalLavrinenko',
 'medalEkins',
 'medalWittmann',
 'medalOrlik',
 'medalOskin',
 'medalHalonen',
 'medalBurda',
 'medalBillotte',
 'medalKolobanov',
 'medalFadin',
 'medalRadleyWalters',
 'medalBrunoPietro',
 'medalTarczay',
 'medalPascucci',
 'medalDumitru',
 'medalLehvaslaiho',
 'medalNikolas',
 'medalLafayettePool',
 'sinai',
 'heroesOfRassenay',
 'mechanicEngineerStrg',
 'beasthunter',
 'mousebane',
 'tankExpertStrg',
 'titleSniper',
 'invincible',
 'diehard',
 'raider',
 'handOfDeath',
 'armorPiercer',
 'kamikaze',
 'lumberjack',
 'company/xp',
 'company/battlesCount',
 'company/wins',
 'company/losses',
 'company/survivedBattles',
 'company/frags',
 'company/shots',
 'company/hits',
 'company/spotted',
 'company/damageDealt',
 'company/damageReceived',
 'company/capturePoints',
 'company/droppedCapturePoints',
 'clan/xp',
 'clan/battlesCount',
 'clan/wins',
 'clan/losses',
 'clan/survivedBattles',
 'clan/frags',
 'clan/shots',
 'clan/hits',
 'clan/spotted',
 'clan/damageDealt',
 'clan/damageReceived',
 'clan/capturePoints',
 'clan/droppedCapturePoints',
 'tankExpert',
 'tankExpert0',
 'tankExpert1',
 'tankExpert2',
 'tankExpert3',
 'tankExpert4',
 'tankExpert5',
 'tankExpert6',
 'tankExpert7',
 'tankExpert8',
 'tankExpert9',
 'tankExpert10',
 'tankExpert11',
 'tankExpert12',
 'tankExpert13',
 'tankExpert14',
 'mechanicEngineer',
 'mechanicEngineer0',
 'mechanicEngineer0',
 'mechanicEngineer1',
 'mechanicEngineer2',
 'mechanicEngineer3',
 'mechanicEngineer4',
 'mechanicEngineer5',
 'mechanicEngineer6',
 'mechanicEngineer7',
 'mechanicEngineer8',
 'mechanicEngineer9',
 'mechanicEngineer10',
 'mechanicEngineer11',
 'mechanicEngineer12',
 'mechanicEngineer13',
 'mechanicEngineer14',
 'medalBrothersInArms',
 'medalCrucialContribution',
 'medalDeLanglade',
 'medalTamadaYoshio',
 'bombardier',
 'huntsman',
 'alaric',
 'sturdy',
 'ironMan',
 'luckyDevil',
 'pattonValley',
 'fragsPatton')
VEH_INTERACTION_DETAILS_LEGACY = ('spotted',
 'killed',
 'hits',
 'he_hits',
 'pierced',
 'damageDealt',
 'damageAssisted',
 'crits',
 'fire')
VEH_INTERACTION_DETAILS_INDICES_LEGACY = dict(((x[1], x[0]) for x in enumerate(VEH_INTERACTION_DETAILS_LEGACY)))
VEH_INTERACTION_DETAILS = (('spotted',
  'B',
  1,
  0),
 ('deathReason',
  'b',
  10,
  -1),
 ('hits',
  'H',
  65535,
  0),
 ('he_hits',
  'H',
  65535,
  0),
 ('pierced',
  'H',
  65535,
  0),
 ('damageDealt',
  'H',
  65535,
  0),
 ('damageAssistedTrack',
  'H',
  65535,
  0),
 ('damageAssistedRadio',
  'H',
  65535,
  0),
 ('crits',
  'I',
  4294967295L,
  0),
 ('fire',
  'H',
  65535,
  0))
VEH_INTERACTION_DETAILS_NAMES = [ x[0] for x in VEH_INTERACTION_DETAILS ]
VEH_INTERACTION_DETAILS_MAX_VALUES = dict(((x[0], x[2]) for x in VEH_INTERACTION_DETAILS))
VEH_INTERACTION_DETAILS_INIT_VALUES = [ x[3] for x in VEH_INTERACTION_DETAILS ]
VEH_INTERACTION_DETAILS_LAYOUT = ''.join([ x[1] for x in VEH_INTERACTION_DETAILS ])
VEH_INTERACTION_DETAILS_INDICES = dict(((x[1][0], x[0]) for x in enumerate(VEH_INTERACTION_DETAILS)))

def usage():
    print str(sys.argv[0]) + ' battleresult.dat [options]'
    print 'Options:'
    print '-f Formats the JSON to be more human readable'
    print '-s Server Mode, disable writing of timestamp, enable logging'



def main():
    global option_server
    global filename_target
    global filename_source
    global option_format
    import cPickle
    import struct
    import json
    import time
    import sys
    import os
    import shutil
    import datetime
    parserversion = '0.8.6.0'
    option_raw = 0
    option_format = 0
    option_server = 0
    option_frags = 1
    if len(sys.argv) == 1:
        usage()
        sys.exit(2)
    for argument in sys.argv:
        if argument == '-r':
            option_raw = 1
        elif argument == '-f':
            option_format = 1
        elif argument == '-s':
            option_server = 1

    filename_source = str(sys.argv[1])
    printmessage('###### WoTBR2J ' + parserversion)
    printmessage('Processing ' + filename_source)
    if option_server == 0:
        tanksdata = get_json_data('tanks.json')
        mapdata = get_json_data('maps.json')
    if not os.path.exists(filename_source) or not os.path.isfile(filename_source) or not os.access(filename_source, os.R_OK):
        exitwitherror('Battle Result does not exists!')
        sys.exit(1)
    filename_target = os.path.splitext(filename_source)[0]
    filename_target = filename_target + '.json'
    cachefile = open(filename_source, 'rb')
    try:
        (batteresultversion, battleResults,) = cPickle.load(cachefile)
    except Exception as e:
        exitwitherror('Battle Result cannot be read (pickle could not be read) ' + e.message)
        sys.exit(1)
    if 'battleResults' not in locals():
        exitwitherror('Battle Result cannot be read (battleResults does not exist)')
        sys.exit(1)
    if len(battleResults[1]) == 50:
        batteresultversion = 1
    elif len(battleResults[1]) == 52:
        batteresultversion = 2
    elif len(battleResults[1]) == 60:
        batteresultversion = 3
    else:
        batteresultversion = 0
    printmessage('Version:' + str(batteresultversion))
    bresult = convertToFullForm(battleResults)
    tanksource = bresult['personal']['typeCompDescr']
    bresult['personal']['tankID'] = tanksource >> 8 & 65535
    bresult['personal']['countryID'] = tanksource >> 4 & 15
    if option_server == 0:
        bresult['personal']['tankName'] = get_tank_data(tanksdata, bresult['personal']['countryID'], bresult['personal']['tankID'], 'title')
    bresult['personal']['won'] = True if bresult['common']['winnerTeam'] == bresult['personal']['team'] else False
    achievements = list()
    for achievementID in bresult['personal']['achievements']:
        achievements.append(ACHIEVEMENTS[achievementID])

    bresult['personal']['achievementlist'] = achievements
    for (key, value,) in bresult['vehicles'].items():
        if len(battleResults[1]) < 60:
            bresult['vehicles'][key]['details'] = VehicleInteractionDetails_LEGACY.fromPacked(value['details']).toDict()
        else:
            bresult['vehicles'][key]['details'] = VehicleInteractionDetails.fromPacked(value['details']).toDict()
        tanksource = bresult['vehicles'][key]['typeCompDescr']
        if tanksource == None:
            bresult['vehicles'][key]['tankID'] = -1
            bresult['vehicles'][key]['countryID'] = -1
            bresult['vehicles'][key]['tankName'] = 'unknown'
        else:
            bresult['vehicles'][key]['tankID'] = tanksource >> 8 & 65535
            bresult['vehicles'][key]['countryID'] = tanksource >> 4 & 15
            if option_server == 0:
                bresult['vehicles'][key]['tankName'] = get_tank_data(tanksdata, bresult['vehicles'][key]['countryID'], bresult['vehicles'][key]['tankID'], 'title')
            else:
                bresult['vehicles'][key]['tankName'] = '-'

    for (key, value,) in bresult['players'].items():
        bresult['players'][key]['platoonID'] = bresult['players'][key]['prebattleID']
        del bresult['players'][key]['prebattleID']
        for (vkey, vvalue,) in bresult['vehicles'].items():
            if bresult['vehicles'][vkey]['accountDBID'] == key:
                bresult['players'][key]['vehicleid'] = vkey
                break


    bresult['common']['bonusTypeName'] = ARENA_GAMEPLAY_NAMES[bresult['common']['bonusType']]
    gameplayID = bresult['common']['arenaTypeID'] // 256
    mapID = bresult['common']['arenaTypeID'] & 32767
    gameplayName = 'ctf'
    if gameplayID == 256:
        gameplayName = 'domination'
    elif gameplayID == 512:
        gameplayName = 'assault'
    bresult['common']['gameplayID'] = gameplayID
    bresult['common']['gameplayName'] = gameplayName
    bresult['common']['arenaTypeID'] = mapID
    if option_server == 0:
        bresult['common']['arenaTypeName'] = get_map_data(mapdata, mapID, 'mapname')
        bresult['common']['arenaTypeIcon'] = get_map_data(mapdata, mapID, 'mapidname')
    bresult['parser'] = 'http://www.vbaddict.net'
    bresult['parserversion'] = parserversion
    bresult['parsertime'] = time.mktime(time.localtime())
    bresult['common']['arenaCreateTimeH'] = datetime.datetime.fromtimestamp(int(bresult['common']['arenaCreateTime'])).strftime('%Y-%m-%d %H:%M:%S')
    bresult['batteresultversion'] = batteresultversion
    bresult['common']['finishReasonName'] = FINISH_REASON_NAMES[(bresult['common']['finishReason'] - 1)]
    bresult['common']['result'] = 'ok'
    dumpjson(bresult)
    printmessage('###### Done!')
    printmessage('')
    sys.exit(0)



def exitwitherror(message):
    catch_fatal(message)
    dossierheader = dict()
    dossierheader['common'] = dict()
    dossierheader['common']['result'] = 'error'
    dossierheader['common']['message'] = message
    dumpjson(dossierheader)
    sys.exit(1)



def dumpjson(bresult):
    if option_server == 1:
        print json.dumps(bresult)
    else:
        finalfile = open(filename_target, 'w')
        if option_format == 1:
            finalfile.write(json.dumps(bresult, sort_keys=True, indent=4))
        else:
            finalfile.write(json.dumps(bresult))



def dictToList(indices, d):
    l = [None] * len(indices)
    for (name, index,) in indices.iteritems():
        l[index] = d[name]

    return l



def listToDict(names, l):
    d = {}
    for x in enumerate(names):
        d[x[1]] = l[x[0]]

    return d



def convertToFullForm(compactForm):
    if len(compactForm[1]) == 50:
        VEH_CELL_RESULTS = ('health', 'credits', 'xp', 'shots', 'hits', 'he_hits', 'pierced', 'damageDealt', 'damageAssisted', 'damageReceived', 'shotsReceived', 'spotted', 'damaged', 'kills', 'tdamageDealt', 'tkills', 'isTeamKiller', 'capturePoints', 'droppedCapturePoints', 'mileage', 'lifeTime', 'killerID', 'achievements', 'repair', 'freeXP', 'details', 'potentialDamageDealt', 'potentialDamageReceived', 'soloHitsAssisted', 'isEnemyBaseCaptured', 'stucks', 'autoAimedShots', 'presenceTime', 'spot_list', 'damage_list', 'kill_list', 'ammo', 'crewActivityFlags', 'series', 'tkillRating', 'tkillLog', 'hasTHit')
        VEH_CELL_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_CELL_RESULTS)))
        VEH_BASE_RESULTS = VEH_CELL_RESULTS[:VEH_CELL_RESULTS.index('potentialDamageDealt')] + ('accountDBID', 'team', 'typeCompDescr', 'gold', 'xpPenalty', 'creditsPenalty', 'creditsContributionIn', 'creditsContributionOut', 'eventIndices', 'vehLockTimeFactor') + VEH_CELL_RESULTS[VEH_CELL_RESULTS.index('potentialDamageDealt'):]
        VEH_BASE_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_BASE_RESULTS)))
        VEH_PUBLIC_RESULTS = VEH_BASE_RESULTS[:VEH_BASE_RESULTS.index('xpPenalty')]
        VEH_PUBLIC_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_PUBLIC_RESULTS)))
        VEH_FULL_RESULTS = VEH_BASE_RESULTS[:VEH_BASE_RESULTS.index('eventIndices')] + ('tmenXP', 'eventCredits', 'eventGold', 'eventXP', 'eventFreeXP', 'eventTMenXP', 'autoRepairCost', 'autoLoadCost', 'autoEquipCost', 'isPremium', 'premiumXPFactor10', 'premiumCreditsFactor10', 'dailyXPFactor10', 'aogasFactor10', 'markOfMastery', 'dossierPopUps')
        VEH_FULL_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_FULL_RESULTS)))
        PLAYER_INFO = ('name', 'clanDBID', 'clanAbbrev', 'prebattleID', 'team')
        PLAYER_INFO_INDICES = dict(((x[1], x[0]) for x in enumerate(PLAYER_INFO)))
        COMMON_RESULTS = ('arenaTypeID', 'arenaCreateTime', 'winnerTeam', 'finishReason', 'duration', 'bonusType', 'vehLockMode')
        COMMON_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(COMMON_RESULTS)))
    if len(compactForm[1]) == 52:
        VEH_CELL_RESULTS = ('health', 'credits', 'xp', 'shots', 'hits', 'thits', 'he_hits', 'pierced', 'damageDealt', 'damageAssisted', 'damageReceived', 'shotsReceived', 'spotted', 'damaged', 'kills', 'tdamageDealt', 'tkills', 'isTeamKiller', 'capturePoints', 'droppedCapturePoints', 'mileage', 'lifeTime', 'killerID', 'achievements', 'potentialDamageReceived', 'repair', 'freeXP', 'details', 'potentialDamageDealt', 'soloHitsAssisted', 'isEnemyBaseCaptured', 'stucks', 'autoAimedShots', 'presenceTime', 'spot_list', 'damage_list', 'kill_list', 'ammo', 'crewActivityFlags', 'series', 'tkillRating', 'tkillLog')
        VEH_CELL_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_CELL_RESULTS)))
        VEH_BASE_RESULTS = VEH_CELL_RESULTS[:VEH_CELL_RESULTS.index('potentialDamageDealt')] + ('accountDBID', 'team', 'typeCompDescr', 'gold', 'xpPenalty', 'creditsPenalty', 'creditsContributionIn', 'creditsContributionOut', 'eventIndices', 'vehLockTimeFactor') + VEH_CELL_RESULTS[VEH_CELL_RESULTS.index('potentialDamageDealt'):]
        VEH_BASE_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_BASE_RESULTS)))
        VEH_PUBLIC_RESULTS = VEH_BASE_RESULTS[:VEH_BASE_RESULTS.index('xpPenalty')]
        VEH_PUBLIC_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_PUBLIC_RESULTS)))
        VEH_ACCOUNT_RESULTS = ('tmenXP', 'eventCredits', 'eventGold', 'eventXP', 'eventFreeXP', 'eventTMenXP', 'autoRepairCost', 'autoLoadCost', 'autoEquipCost', 'isPremium', 'premiumXPFactor10', 'premiumCreditsFactor10', 'dailyXPFactor10', 'aogasFactor10', 'markOfMastery', 'dossierPopUps')
        VEH_ACCOUNT_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_ACCOUNT_RESULTS)))
        VEH_FULL_RESULTS = VEH_BASE_RESULTS[:VEH_BASE_RESULTS.index('eventIndices')] + VEH_ACCOUNT_RESULTS
        VEH_FULL_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_FULL_RESULTS)))
        VEH_ACCOUNT_RESULTS_START_INDEX = VEH_FULL_RESULTS_INDICES['tmenXP']
        PLAYER_INFO = ('name', 'clanDBID', 'clanAbbrev', 'prebattleID', 'team')
        PLAYER_INFO_INDICES = dict(((x[1], x[0]) for x in enumerate(PLAYER_INFO)))
        COMMON_RESULTS = ('arenaTypeID', 'arenaCreateTime', 'winnerTeam', 'finishReason', 'duration', 'bonusType', 'guiType', 'vehLockMode')
        COMMON_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(COMMON_RESULTS)))
    if len(compactForm[1]) == 60:
        VEH_CELL_RESULTS = ('health', 'credits', 'xp', 'shots', 'hits', 'thits', 'he_hits', 'pierced', 'damageDealt', 'damageAssistedRadio', 'damageAssistedTrack', 'damageReceived', 'shotsReceived', 'noDamageShotsReceived', 'heHitsReceived', 'piercedReceived', 'spotted', 'damaged', 'kills', 'tdamageDealt', 'tkills', 'isTeamKiller', 'capturePoints', 'droppedCapturePoints', 'mileage', 'lifeTime', 'killerID', 'achievements', 'potentialDamageReceived', 'repair', 'freeXP', 'details', 'potentialDamageDealt', 'soloHitsAssisted', 'isEnemyBaseCaptured', 'stucks', 'autoAimedShots', 'presenceTime', 'spot_list', 'damage_list', 'kill_list', 'ammo', 'crewActivityFlags', 'series', 'tkillRating', 'tkillLog', 'destroyedObjects')
        VEH_CELL_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_CELL_RESULTS)))
        VEH_BASE_RESULTS = VEH_CELL_RESULTS[:VEH_CELL_RESULTS.index('potentialDamageDealt')] + ('accountDBID', 'team', 'typeCompDescr', 'gold', 'deathReason', 'xpPenalty', 'creditsPenalty', 'creditsContributionIn', 'creditsContributionOut', 'eventIndices', 'vehLockTimeFactor', 'misc') + VEH_CELL_RESULTS[VEH_CELL_RESULTS.index('potentialDamageDealt'):]
        VEH_BASE_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_BASE_RESULTS)))
        VEH_PUBLIC_RESULTS = VEH_BASE_RESULTS[:VEH_BASE_RESULTS.index('xpPenalty')]
        VEH_PUBLIC_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_PUBLIC_RESULTS)))
        VEH_ACCOUNT_RESULTS = ('originalCredits', 'originalXP', 'originalFreeXP', 'tmenXP', 'eventCredits', 'eventGold', 'eventXP', 'eventFreeXP', 'eventTMenXP', 'autoRepairCost', 'autoLoadCost', 'autoEquipCost', 'isPremium', 'premiumXPFactor10', 'premiumCreditsFactor10', 'dailyXPFactor10', 'aogasFactor10', 'markOfMastery', 'dossierPopUps')
        VEH_ACCOUNT_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_ACCOUNT_RESULTS)))
        VEH_FULL_RESULTS = VEH_BASE_RESULTS[:VEH_BASE_RESULTS.index('eventIndices')] + VEH_ACCOUNT_RESULTS
        VEH_FULL_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(VEH_FULL_RESULTS)))
        VEH_ACCOUNT_RESULTS_START_INDEX = VEH_FULL_RESULTS_INDICES['originalCredits']
        PLAYER_INFO = ('name', 'clanDBID', 'clanAbbrev', 'prebattleID', 'team')
        PLAYER_INFO_INDICES = dict(((x[1], x[0]) for x in enumerate(PLAYER_INFO)))
        COMMON_RESULTS = ('arenaTypeID', 'arenaCreateTime', 'winnerTeam', 'finishReason', 'duration', 'bonusType', 'guiType', 'vehLockMode')
        COMMON_RESULTS_INDICES = dict(((x[1], x[0]) for x in enumerate(COMMON_RESULTS)))
    fullForm = {'arenaUniqueID': compactForm[0],
     'personal': listToDict(VEH_FULL_RESULTS, compactForm[1]),
     'common': {},
     'players': {},
     'vehicles': {}}
    if len(compactForm[1]) < 60:
        fullForm['personal']['details'] = VehicleInteractionDetails_LEGACY.fromPacked(fullForm['personal']['details']).toDict()
    else:
        fullForm['personal']['details'] = VehicleInteractionDetails.fromPacked(fullForm['personal']['details']).toDict()
    (commonAsList, playersAsList, vehiclesAsList,) = cPickle.loads(compactForm[2])
    fullForm['common'] = listToDict(COMMON_RESULTS, commonAsList)
    for (accountDBID, playerAsList,) in playersAsList.iteritems():
        fullForm['players'][accountDBID] = listToDict(PLAYER_INFO, playerAsList)

    for (vehicleID, vehicleAsList,) in vehiclesAsList.iteritems():
        fullForm['vehicles'][vehicleID] = listToDict(VEH_PUBLIC_RESULTS, vehicleAsList)

    return fullForm



def printmessage(message):
    if option_server == 0:
        print message



def catch_fatal(message):
    import shutil
    printmessage(message)



def write_to_log(logtext):
    import datetime
    import os
    print logtext
    now = datetime.datetime.now()
    if option_server == 1:
        logFile = open('/var/log/wotdc2j/wotdc2j.log', 'a+b')
        logFile.write(str(now.strftime('%Y-%m-%d %H:%M:%S')) + ' # ' + str(logtext) + ' # ' + str(filename_source) + '\r\n')
        logFile.close()



def get_json_data(filename):
    import json
    import time
    import sys
    import os
    os.chdir(sys.path[0])
    if not os.path.exists(filename) or not os.path.isfile(filename) or not os.access(filename, os.R_OK):
        catch_fatal(filename + ' does not exists!')
        sys.exit(1)
    file_json = open(filename, 'r')
    try:
        file_data = json.load(file_json)
    except Exception as e:
        catch_fatal(filename + ' cannot be loaded as JSON: ' + e.message)
        sys.exit(1)
    file_json.close()
    return file_data



def get_tank_data(tanksdata, countryid, tankid, dataname):
    for tankdata in tanksdata:
        if tankdata['countryid'] == countryid:
            if tankdata['tankid'] == tankid:
                return tankdata[dataname]

    return 'unknown'



def get_map_data(mapsdata, mapid, dataname):
    for mapdata in mapsdata:
        if mapdata['mapid'] == mapid:
            return mapdata[dataname]

    return 'unknown'



class _VehicleInteractionDetailsItem(object):

    def __init__(self, values, offset):
        self.__values = values
        self.__offset = offset



    def __getitem__(self, key):
        return self.__values[(self.__offset + VEH_INTERACTION_DETAILS_INDICES[key])]



    def __setitem__(self, key, value):
        self.__values[self.__offset + VEH_INTERACTION_DETAILS_INDICES[key]] = min(int(value), VEH_INTERACTION_DETAILS_MAX_VALUES[key])



    def __str__(self):
        return str(dict(self))



    def __iter__(self):
        return izip(VEH_INTERACTION_DETAILS_NAMES, self.__values[self.__offset:])




class VehicleInteractionDetails(object):

    def __init__(self, vehicleIDs, values):
        self.__vehicleIDs = vehicleIDs
        self.__values = values
        size = len(VEH_INTERACTION_DETAILS)
        self.__offsets = dict(((x[1], x[0] * size) for x in enumerate(self.__vehicleIDs)))



    @staticmethod
    def fromPacked(packed):
        count = len(packed) / struct.calcsize(''.join(['<I', VEH_INTERACTION_DETAILS_LAYOUT]))
        packedVehIDsLayout = '<%dI' % (count,)
        packedVehIDsLen = struct.calcsize(packedVehIDsLayout)
        vehicleIDs = struct.unpack(packedVehIDsLayout, packed[:packedVehIDsLen])
        values = struct.unpack('<' + VEH_INTERACTION_DETAILS_LAYOUT * count, packed[packedVehIDsLen:])
        return VehicleInteractionDetails(vehicleIDs, values)



    def __getitem__(self, vehicleID):
        offset = self.__offsets.get(vehicleID, None)
        if offset is None:
            self.__vehicleIDs.append(vehicleID)
            offset = len(self.__values)
            self.__values += VEH_INTERACTION_DETAILS_INIT_VALUES
            self.__offsets[vehicleID] = offset
        return _VehicleInteractionDetailsItem(self.__values, offset)



    def __contains__(self, vehicleID):
        return vehicleID in self.__offsets



    def __str__(self):
        return str(self.toDict())



    def pack(self):
        count = len(self.__vehicleIDs)
        packed = struct.pack(('<%dI' % count), *self.__vehicleIDs) + struct.pack(('<' + VEH_INTERACTION_DETAILS_LAYOUT * count), *self.__values)
        return packed



    def toDict(self):
        return dict([ (vehID, dict(_VehicleInteractionDetailsItem(self.__values, offset))) for (vehID, offset,) in self.__offsets.iteritems() ])




class _VehicleInteractionDetailsItem_LEGACY(object):

    def __init__(self, values, offset):
        self.__values = values
        self.__offset = offset



    def __getitem__(self, key):
        return self.__values[(self.__offset + VEH_INTERACTION_DETAILS_INDICES_LEGACY[key])]



    def __setitem__(self, key, value):
        self.__values[self.__offset + VEH_INTERACTION_DETAILS_INDICES_LEGACY[key]] = min(int(value), 65535)



    def __str__(self):
        return str(dict(self))



    def __iter__(self):
        return izip(VEH_INTERACTION_DETAILS_LEGACY, self.__values[self.__offset:])




class VehicleInteractionDetails_LEGACY(object):

    def __init__(self, vehicleIDs, values):
        self.__vehicleIDs = vehicleIDs
        self.__values = values
        size = len(VEH_INTERACTION_DETAILS_LEGACY)
        self.__offsets = dict(((x[1], x[0] * size) for x in enumerate(self.__vehicleIDs)))



    @staticmethod
    def fromPacked(packed):
        size = len(VEH_INTERACTION_DETAILS_LEGACY)
        count = len(packed) / struct.calcsize('I%dH' % size)
        unpacked = struct.unpack('%dI%dH' % (count, count * size), packed)
        vehicleIDs = unpacked[:count]
        values = unpacked[count:]
        return VehicleInteractionDetails_LEGACY(vehicleIDs, values)



    def __getitem__(self, vehicleID):
        offset = self.__offsets.get(vehicleID, None)
        if offset is None:
            self.__vehicleIDs.append(vehicleID)
            offset = len(self.__values)
            size = len(VEH_INTERACTION_DETAILS_LEGACY)
            self.__values += [0] * size
            self.__offsets[vehicleID] = offset
        return _VehicleInteractionDetailsItem_LEGACY(self.__values, offset)



    def __contains__(self, vehicleID):
        return vehicleID in self.__offsets



    def __str__(self):
        return str(self.toDict())



    def pack(self):
        count = len(self.__vehicleIDs)
        size = len(VEH_INTERACTION_DETAILS_LEGACY)
        packed = struct.pack(('%dI' % count), *self.__vehicleIDs) + struct.pack(('%dH' % count * size), *self.__values)
        return packed



    def toDict(self):
        return dict([ (vehID, dict(_VehicleInteractionDetailsItem_LEGACY(self.__values, offset))) for (vehID, offset,) in self.__offsets.iteritems() ])



if __name__ == '__main__':
    main()

#+++ okay decompyling wotbr2j.pyc 
# decompiled 1 files: 1 okay, 0 failed, 0 verify failed
# 2013.06.07 15:33:03 Kaliningrad Standard Time
