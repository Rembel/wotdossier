# 2013.06.06 18:35:41 Kaliningrad Standard Time
import cPickle
import struct
import json
import time
import sys
import os
import shutil
import datetime

def main():
    parserversion = '0.8.3.0'
    print '###### WoT-Replay-BattleResult-To-JSON ' + parserversion + ' by vBAddict.net'
    hellomessage = '## Hello, nice to meet you. ##'
    filename_source = str(sys.argv[1])
    returndict = dict()
    print 'Processing ' + filename_source
    blocks = dict()
    blocks['common'] = dict()
    blocks['common']['parser'] = 'WoT-Replay-BattleResult-To-JSON ' + parserversion + ' by http://www.vbaddict.net'
    if not os.path.exists(filename_source) or not os.path.isfile(filename_source) or not os.access(filename_source, os.R_OK):
        blocks['common']['message'] = 'cannot read file ' + filename_source
        dumpjson(blocks, filename_source, 1)
    f = open(filename_source, 'rb')
    try:
        f.seek(4)
        numofblocks = struct.unpack('I', f.read(4))[0]
        print 'Found Blocks: ' + str(numofblocks)
        blockNum = 1
        datablockPointer = {}
        datablockSize = {}
        startPointer = 8
    except Exception as e:
        blocks['common']['message'] = e.message
        dumpjson(blocks, filename_source, 1)
    if numofblocks == 0:
        blocks['common']['message'] = 'unknown file structure'
        dumpjson(blocks, filename_source, 1)
    if numofblocks > 4:
        blocks['common']['message'] = 'unknown file structure'
        dumpjson(blocks, filename_source, 1)
    while numofblocks >= 1:
        try:
            f.seek(startPointer)
            size = f.read(4)
            datablockSize[blockNum] = struct.unpack('I', size)[0]
            datablockPointer[blockNum] = startPointer + 4
            startPointer = datablockPointer[blockNum] + datablockSize[blockNum]
            blockNum += 1
            numofblocks -= 1
            for i in datablockSize:
                f.seek(datablockPointer[i])
                myblock = f.read(int(datablockSize[i]))
                if 'arenaUniqueID' in myblock:
                    returndict = cPickle.loads(myblock)
                    for mindex in enumerate(returndict['vehicles']):
                        del returndict['vehicles'][mindex[1]]['details']

                    blocks['datablock_battle_result'] = returndict
                else:
                    blockdict = dict()
                    blockdict = json.loads(myblock)
                    blocks['datablock_' + str(i)] = blockdict

            blocks['common']['message'] = 'ok'
        except Exception as e:
            blocks['common']['message'] = e.message
            dumpjson(blocks, filename_source, 1)

    dumpjson(blocks, filename_source, 0)



def dumpjson(mydict, filename_source, exitcode):
    if exitcode == 0:
        mydict['common']['status'] = 'ok'
    else:
        mydict['common']['status'] = 'error'
    filename_target = os.path.splitext(filename_source)[0]
    filename_target = filename_target + '.json'
    finalfile = open(filename_target, 'w')
    finalfile.write(json.dumps(mydict, sort_keys=True, indent=4))
    sys.exit(exitcode)


if __name__ == '__main__':
    main()

#+++ okay decompyling wotrpbr2j.pyc 
# decompiled 1 files: 1 okay, 0 failed, 0 verify failed
# 2013.06.06 18:35:41 Kaliningrad Standard Time
