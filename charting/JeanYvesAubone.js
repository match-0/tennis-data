var hand = 'R';

<html><head>
<title>Tennis Abstract: Jean Yves Aubone Match Results, Splits, and Analysis</title>
<link rel="stylesheet" href="https://www.tennisabstract.com/blue/style.css" type="text/css">
<script type="text/javascript" src="https://www.tennisabstract.com/jquery-1.7.1-min.js"></script>
<script type="text/javascript" src="https://www.tennisabstract.com/jquery.tablesorter.js"></script>
<script type="text/javascript" src="https://www.tennisabstract.com/navbar.js"></script>
<script type="text/javascript" src="https://www.minorleaguesplits.com/tennisabstract/cgi-bin/jsplayers/curr_rank_atp.js"></script>
<script type="text/javascript" src="https://www.minorleaguesplits.com/tennisabstract/cgi-bin/jsmatches/JeanYvesAubone.js"></script>
<script type="text/javascript" src="https://www.minorleaguesplits.com/tennisabstract/cgi-bin/jsdoubles/JeanYvesAubone.js"></script>
<script language="JavaScript">
var currentTime = new Date();
var month = currentTime.getMonth() + 1;
var day = currentTime.getDate();
var year = currentTime.getFullYear().toString();
var mm, dd;
if (month < 10) {mm = '0' + month.toString();}
else {mm = month.toString();}
if (day < 10) {dd = '0' + day.toString();}
else {dd = day.toString();}
var today = year + mm + dd;
var one_day=1000*60*60*24;
var nameparam = 'JeanYvesAubone';
var prefilters = {};
var opponent="";
var exclude="";
var opp_team="";
var partner="";
var view="";
var keep_loading = 1;


var fourspaces = "\u00a0\u00a0\u00a0\u00a0";

var round_dict = { "R16": 9,
                   "W": 14,
                   "F": 13,
                   "RR": 8,
                   "R64": 6,
                   "R128": 5,
                   "QF": 10,
                   "SF": 11,
                   "R32": 7,
                   'Q1': 1,
                   'Q2': 2,
                   'Q3': 3,
                   'Q4': 4,
                   "": 0,
                   "BR": 12
                  };

// add parser through the tablesorter addParser method 
$.tablesorter.addParser({ 
    // set a unique id 
    id: 'rounds', 
    is: function(s) { 
        // return false so this parser is not auto detected 
        return false; 
    }, 
    format: function(s) { 
        // format your data for normalization 
        return round_dict[s]; 
    }, 
    // set type, either numeric or text 
    type: 'numeric'
});

$.tablesorter.addParser({ 
    id: 'roundsDesc', 
    is: function(s) {  
        return false; 
    }, 
    format: function(s) {  
        return 100 - round_dict[s]; 
    }, 
    type: 'numeric'
});

$.tablesorter.addParser({  
    id: 'scores', 
    is: function(s) {return false;}, 
    format: function(s) {
        if (s == 'W/O') {return 0;}
        if (s.slice(-3) == 'RET') {s = s.slice(0, -4);}        
        if (s == 'UNR') {return 2000;}
        else {return s;}
    },  
    type: 'numeric'
});

$.tablesorter.addParser({  
    id: 'ranks', 
    is: function(s) {return false;}, 
    format: function(s) { 
        if (s == 'UNR') {return 3000;}
        else if (s.indexOf('/') !== -1) {
            var dranks = s.split(/\//);
            if (dranks[0] == "UNR") {var rank1 = 3000;}
            else {rank1 = parseInt(dranks[0]);}
            if (dranks[1] == "UNR") {var rank2 = 3000;}
            else {rank2 = parseInt(dranks[1]);}
            return rank1 + rank2;
            }
        else {return s;} 
    },  
    type: 'numeric'
});

$.tablesorter.addParser({  
    id: 'ascNum', 
    is: function(s) {return false;}, 
    format: function(s) { 
        if (s == '' || s == '-') {return 1000;} 
        else if (s.indexOf("%") != -1) {return parseFloat(s.slice(0,-1));}
        else if (s.indexOf(":") != -1) {
            var hm = s.split(':');
            var minutes = (parseInt(hm[0])*60) + parseInt(hm[1]);
            return minutes;
            }
        else {return s;} 
    },  
    type: 'numeric'
});

$.tablesorter.addParser({  
    id: 'descNum', 
    is: function(s) {return false;}, 
    format: function(s) { 
        if (s == '' || s == '-') {return 1000;} 
        else if (s.indexOf("%") != -1) {return 1000 - parseFloat(s.slice(0,-1));}
        else if (s.indexOf(":") != -1) {
            var hm = s.split(':');
            var minutes = (parseInt(hm[0])*60) + parseInt(hm[1]);
            return 1000 - minutes;
            }        
        else {return 1000 - s;} 
    },  
    type: 'numeric'
});

$.tablesorter.addParser({  
    id: 'dates', 
    is: function(s) {return false;}, 
    format: function(s) { 
        // format your data for normalization
        var yyyy = s.slice(-4);
        var mm = rmonths[s.slice(-8, -5)];
        var day = s.slice(0, -9);
        if (day.length == 1) {
            var dd = '0' + day;
            }
        else {
            var dd = day;
            }
        return yyyy + mm + dd; 
    },  
    type: 'numeric'
}); 

$.tablesorter.addParser({ 
    id: 'datesDesc', 
    is: function(s) {return false;}, 
    format: function(s) { 
        var yyyy = s.slice(-4);
        var mm = rmonths[s.slice(-8, -5)];
        var day = s.slice(0, -9);
        if (day.length == 1) {
            var dd = '0' + day;
            }
        else {
            var dd = day;
            }
        var ymd = yyyy + mm + dd; 
        return 100000000 - parseInt(ymd);
    },  
    type: 'numeric'
});
 
function playeratt(pname, s, q, cc, lk) {
    // lk = 1 or 0
    var precede = ''
    if (s != '') {
        precede = '(' + s;
        if (q != '') {
            precede = precede + '/' + q;
            }
        precede = precede + ')';
        }
    else if (q != '') {
        precede = '(' + q + ')';
        }
    //var nlink = '<a href="http://www.tennisabstract.com/cgi-bin/player.cgi?p=' + pname.replace(/ /g, '') + '">' + pname + '</a>'
    if (cc == '') {var country = '';}
    else {var country = ' [' + cc + ']';}
    if (lk == 1) {return precede + nlink + country;}
    else {return precede + pname + country;}
    
    };

function alignRound(num, dec, perc) {
    if (perc == 1) {
        num = num*100;
        }
    var intrate = Math.round((num)*Math.pow(10, dec))/Math.pow(10, dec);
    var extra = intrate + Math.pow(10, -1*(dec+1));
    var strex = String(extra);
    var indexdot = strex.indexOf('.');
    var done;
    if (perc == 1) {done = strex.slice(0, (indexdot+dec+1)) + '%';}
    else {done = strex.slice(0, (indexdot+dec+1));}
    if (done[0] == 'N' || done[0] == '%') {return '-';}
    else if (done[0] == 'I') {return '-';} // infinity
    else if (dec == 0) {return done.slice(0, -2) + done.slice(-1);}
    else {return done;}
    };
                  
function make(tag, attr, cont) {
    if (arguments.length == 2 && (attr instanceof Array || typeof attr == "string")) {
        cont = attr;
        attributes = null;
        }
    var open = "<" + tag;
    var close = "</" + tag + ">";
    if (attr) {
	for(var name in attr) {
            attx = " " + name + '="' + attr[name] + '"';
	    open = open.concat(attx);
            }
        }
    open = open.concat(">");
    if (cont instanceof Array) {
	var content = cont.join("");
	}
    else {
	var content = cont;
	} 
    var elem = open + content + close;
    return elem;
    }

function sortRev(a,b) {return b - a;}

url2filter = {'K': 'hand',
              'E': 'round',
              'I': 'rank',
              'L': 'age',
              'C': 'level',
              'B': 'surface',
              'A': 'span', 
              'N': 'country', 
              'D': 'tourney', 
              'G': 'asrank',
              'H': 'entry',
              'J': 'oentry',
              'M': 'height',
              'F': 'results',
              'P': 'sets',
              'Q': 'scores',
              'T': 'stats',
              'X': 'extras',
              'U': 'minimum',
              'R': 'crank',
              'Y': 'minyears',
              's': 'sort',
              'r': 'return',
              'v': 'reverse',
              'o': 'overall',
              'w': 'raw',
              'z': 'hands',
              'S': 'prank',
              'V': 'phand'
              };

filter2url = {'hand': 'K',
              'round': 'E',
              'rank': 'I',
              'age': 'L',
              'level': 'C',
              'surface': 'B',
              'span': 'A', 
              'country': 'N', 
              'tourney': 'D', 
              'asrank': 'G',
              'entry': 'H',
              'oentry': 'J',
              'height': 'M',
              'results': 'F',
              'sets': 'P',
              'scores': 'Q',
              'stats': 'T',
              'extras': 'X',
              'minimum': 'U',
              'crank': 'R',
              'minyears': 'Y',
              'hands': 'Z',
              'prank': 'S',
              'phand': 'V'
              };

var filterlist = ['span', 'minyears', 'minimum', 'surface', 'level', 'tourney', 'round', 'results', 'sets', 'scores', 'asrank', 'entry',
              'rank', 'crank', 'oentry', 'hand', 'hands', 'age', 'height', 'country', 'phand', 'prank', 'stats']; //, 'extras'];

var filteropts = {'hand': ['vs Hand', 'All', 'Right', 'Left', 'One-hand BH', 'Two-hand BH'],
                  'phand': ['Partner Hand', 'All', 'Right', 'Left'],
                  'hands': ['vs Hands', 'All', 'Right/Right', 'Right/Left', 'Left/Left'],
                  'round': ['Round', 'All', 'Final', 'Semis', 'Quarters', 'R16', 'R32', 'R64', 'R128', "First Round", "Second Round", "Third Round", "First Match", "Second Match", "Third Match"],
                  'rank': ['vs Rank', 'All', 'Top 5', 'Top 10', 'Top 20', 'Top 50', 'Top 100', '11+', '21+', '51+', '101+', 'Higher', 'Lower'], 
                  'prank': ['Partner Rank', 'All', 'Top 5', 'Top 10', 'Top 20', 'Top 50', 'Top 100', '11+', '21+', '51+', '101+', 'Higher', 'Lower'], 
                  'age': ['vs Age', 'All', 'Younger', 'Older', 'Under 21', 'Under 23', '28 & Over', '30 & Over'],
                  'level': ['Level', 'All', 'Grand Slams', 'Masters', 'All ATP', 'Qualifying', 'Challengers', 'CH Qualies', 'Futures', 'Davis Cup', 'All Pro', 'Juniors'],
                  'surface': ['Surface', 'All', 'Hard', 'Clay', 'Grass', 'Carpet'], // add indoor!
                  'span': ychoices, 
                  'country': cchoices, 
                  'tourney': tchoices, 
                  'asrank': rchoices,
                  'entry': ['as Entry', 'All', 'Seeded', 'Unseeded', 'Qualifier', 'Wild Card'],
                  'oentry': ['vs Entry', 'All', 'Seeded', 'Unseeded', 'Qualifier', 'Wild Card'],
                  'height': ['vs Height', 'All', 'Shorter', 'Taller', "Under 5'10", "Under 6'0", "Over 6'2", "Over 6'4"],
                  'results': ['Results', 'All', 'Wins', 'Losses', 'Completed', 'W by RET', 'L by RET', 'W by W/O', 'L by W/O', 'Won 1st Set', 'Lost 1st Set', 'Won Sets 1&2', 'Lost Sets 1&2', 'Split 1&2', 'Up 2-1 Sets', 'Down 1\u20112 Sets'],
                  'scores': ['Scores', 'All', 'All tiebreaks', 'TB won', 'TB lost', 'Deciding TB', 'All 7-5', '7-5 won', '7-5 lost', 'All bagels', '6-0 won', '6-0 lost', 'All 6-1', '6-1 won', '6-1 lost'],
                  'sets': ['Sets', 'All', 'Straights', 'Deciders', 'All Best of 5', '3 Sets (of 5)', '4-Setters', '5-Setters', 'All Best of 3', '2-Setters', '3 Sets (of 3)'],
                  'stats': ['Stats', 'All'],
                  'extras': ['Extras', 'All', 'Charting', 'Pt-by-Pt', 'Video'],
                  'minimum': ['Min Matches', 'All', '2', '3', '4', '5', '6', '7', '8', '9', '10', '12', '15'],
                  'minyears': ['Min Years', 'All', '2', '3', '4', '5', '6', '7', '8', '9', '10'],
                  'crank': ['vs Curr Rank', 'All', 'Top 10', 'Top 20', 'Top 50', 'Top 100', 'Active', 'Inactive']
                  };

var rmonths = {'Jan': '01',
                'Feb': '02',
                'Mar': '03',
                'Apr': '04',
                'May': '05',
                'Jun': '06',
                'Jul': '07',
                'Aug': '08',
                'Sep': '09',
                'Oct': '10',
                'Nov': '11',
                'Dec': '12'
                };

var months = {'01': 'Jan',
                '02': 'Feb',
                '03': 'Mar',
                '04': 'Apr',
                '05': 'May',
                '06': 'Jun',
                '07': 'Jul',
                '08': 'Aug',
                '09': 'Sep',
                '10': 'Oct',
                '11': 'Nov',
                '12': 'Dec'
                };

var titleTips = {'DR': 'Dominance Ratio: % of return points won\ndivided by % of serve points lost.',
                 'TPW': 'Total points won',
                 'RPW': 'Return points won',
                 'vA%': 'Ace rate against',
                 'v1st%': '1st serve return points won',
                 'v2nd%': '2nd serve return points won',
                 'BPCnv': 'Break point opportunities converted',
                 'BPSvd': 'Break point opportunities saved',
                 'A%': 'Ace rate',
                 'Ace%': 'Ace rate',
                 'DF%': 'Double fault rate',
                 '1st In': 'First serve percentage',
                 '1st%': 'First serve points won',
                 '2nd%': 'Second serve points won',
                 'Rk': "Player's ATP Ranking when the tournament began",
                 'Ranks': "Player's and partner's ATP Rankings when the tournament began",
                 'vRk': "Opponent's ATP Ranking when the tournament began",
                 'vRanks': "Opponents' ATP Rankings when the tournament began",
                 'TP': 'Total Points',
                 'DFs': 'Double faults',
                 'SP': 'Service points',
                 '1SP': '1st serve points',
                 '2SP': '2nd serve points',
                 'vA': 'Opponent aces',
                 'SPW': 'Service points won',
                 'BPSvd%': 'Percentage of break points saved',
                 'BPCnv%': 'Percentage of break points converted',
                 'MS': 'Number of H2H matches with stats\nincluded in the totals to the right',
                 'M': 'Matches played',
                 'Yrs': 'Number of years entered'
                 };

var hdrsServe = ['Date', 'Tournament', 'Surface', 'Rd', 'Rk', 'vRk', '', 'Score', 'More', 'DR', 'A%', 'DF%', '1stIn', '1st%', '2nd%', 'BPSvd', 'Time'];
var hdrsReturn = ['Date', 'Tournament', 'Surface', 'Rd', 'Rk', 'vRk', '', 'Score', 'More', 'DR', 'TPW', 'RPW', 'vA%', 'v1st%', 'v2nd%', 'BPCnv', 'Time'];
var hdrsRaw = ['Date', 'Tournament', 'Surface', 'Rd', 'Rk', 'vRk', '', 'Score', 'More', 'TP', 'Aces', 'DFs', 'SP', '1SP', '2SP', 'vA', 'Time'];

var hdrsDoubles = ['Date', 'Tournament', 'Surface', 'Rd', 'Ranks', 'vRanks', '', 'Score', 'DR', 'Time'];
var hdrsDoublesServe = ['Date', 'Tournament', 'Surface', 'Rd', '', 'Score', 'DR', 'A%', 'DF%', '1stIn', '1st%', '2nd%', 'BPSvd', 'Time'];
var hdrsDoublesReturn = ['Date', 'Tournament', 'Surface', 'Rd', '', 'Score', 'DR', 'TPW', 'RPW', 'vA%', 'v1st%', 'v2nd%', 'BPCnv', 'Time'];
var hdrsDoublesRaw = ['Date', 'Tournament', 'Surface', 'Rd', '', 'Score', 'TP', 'Aces', 'DFs', 'SP', '1SP', '2SP', 'vA', 'Time'];

var hdrsHead = ['H2Hs', 'Opponent', 'W', 'L', 'Win%', 'TB', 'W', 'L', 'TB%', 'First Match', 'Last Match', 'MS', 
                'DR', 'A%', 'DF%', '1stIn', '1st%', '2nd%', 'SPW', 'RPW', 'BPSvd%', 'BPCnv%'];
var hdrsEvents = ['Yrs', 'Event', 'Surface', 'M', 'W', 'L', 'Win%', 'TB', 'W', 'L', 'TB%', 'First', 'Last', 'Best', 'MS', 
                'DR', 'A%', 'DF%', '1stIn', '1st%', '2nd%', 'SPW', 'RPW', 'BPSvd%', 'BPCnv%'];                

var hdict = {'Left': 'L',
             'Right': 'R'
             };

var hdict2 = {'L': 'Left',
              'R': 'Right',
              '': 'Unknown'
             };
             
var levdict = {'Grand Slams': 'G',
               'Masters': 'M',
               'Challengers': 'C',
               'Qualifying': 'Q',
               'Davis Cup': 'D',
               'Juniors': 'J',
               'Futures': 'S'
               }

var levdict2 = {'G': 'Grand Slams',
               'M': 'Masters',
               //'C': 'Challengers',
               'Q': 'Qualifying',
               'D': 'Davis Cup',
               'J': 'Juniors',
               'S': 'Futures'
               }

var rddict = {'Final': 'F',
              'Semis': 'SF',
              'Quarters': 'QF',
              'R16': 'R16',
              'R32': 'R32',
              'R64': 'R64',
              'R128': 'R128',
              'Other': 'RR'
              }

var rddict2 = {'F': 'Final',
              'SF': 'Semis',
              'QF': 'Quarters',
              'R16': 'R16',
              'R32': 'R32',
              'R64': 'R64',
              'R128': 'R128',
              'RR': 'Other'
              }                            

function filterDict(fid, match, mults) {
    if (!$('#tabDubs').hasClass("tablink")) {var doubles = 1;}
    else {doubles = 0;}
    if (fid == 'hand') {
        if (hdict2[match.ohand] in mults) {return 1;}
        else if (match.obackhand == '1' && 'One-hand BH' in mults) {return 1;}
        else if (match.obackhand == '2' && 'Two-hand BH' in mults) {return 1;}
        else {return 0;}
        }
    if (fid == 'phand') {
        if (hdict2[match.phand] in mults) {return 1;}
        else {return 0;}
        }
    else if (fid == 'hands') {
        if ('Right/Right' in mults) {
            if (match.ohand == 'R' && match.o2hand == 'R') {return 1;}
            }
        if ('Left/Left' in mults) {
            if (match.ohand == 'L' && match.o2hand == 'L') {return 1;}
            }
        if ('Right/Left' in mults) {
            var hh = match.ohand + match.o2hand;
            if (hh == 'RL' || hh == 'LR') {return 1;}
            }
        return 0;
        }
    else if (fid == 'extras') { 
        if ('Charting' in mults && match.chartlink != "") {return 1;}
        else if ('Pt-by-Pt' in mults && match.pslink != "") {return 1;}
        else if ('Video' in mults && match.vidlink != "") {return 1;}
        else {return 0;}
        }
    else if (fid == 'age') {
        if ('Younger' in mults && (parseInt(match.obday)) > dob) {return 1;}
        else if ('Older' in mults && (parseInt(match.obday) < dob)) {return 1;}
        else if ('Under 21' in mults && (parseInt(match.date) - parseInt(match.obday)) < 210000) {return 1;}
        else if ('Under 23' in mults && (parseInt(match.date) - parseInt(match.obday)) < 230000) {return 1;}
        else if ('28 & Over' in mults && (parseInt(match.date) - parseInt(match.obday)) > 280000) {return 1;}
        else if ('30 & Over' in mults && (parseInt(match.date) - parseInt(match.obday)) > 300000) {return 1;}
        else {return 0;}
        }
    else if (fid == 'rank') {
        if (match.orank == 'UNR') {match.orank = 2000;}
        if (match.rank == 'UNR') {match.rank = 2000;}
        if (doubles == 1) {
            if (match.o2rank == 'UNR') {match.o2rank = 2000;}
            var vsrank = (parseInt(match.orank) + parseInt(match.o2rank))/2;
            if (match.prank == 'UNR') {match.prank = 2000;}
            var asrank = (parseInt(match.rank) + parseInt(match.prank))/2;
            }
        else {
            vsrank = parseInt(match.orank);
            asrank = parseInt(match.rank);
            }        
        if ('Top 5' in mults && vsrank < 6) {return 1;}
        else if ('Top 10' in mults && vsrank < 11) {return 1;}
        else if ('Top 20' in mults && vsrank < 21) {return 1;}
        else if ('Top 50' in mults && vsrank < 51) {return 1;}
        else if ('Top 100' in mults && vsrank < 101) {return 1;}
        else if ('11+' in mults && vsrank > 10) {return 1;}
        else if ('21+' in mults && vsrank > 20) {return 1;}
        else if ('51+' in mults && vsrank > 50) {return 1;}
        else if ('101+' in mults && vsrank > 100) {return 1;}
        else if ('Higher' in mults && (vsrank < asrank)) {return 1;}
        else if ('Lower' in mults && (vsrank > asrank)) {return 1;}
        else if ('Custom' in mults) {
            if (vsrank >= lowrank && vsrank <= highrank) {return 1;}
            return 0;
            }
        else {return 0;}
        }
    else if (fid == 'prank') {
        if (match.orank == 'UNR') {match.orank = 2000;}
        if (match.rank == 'UNR') {match.rank = 2000;}
        var vsrank = parseInt(match.prank);  // misleading variable name; partner rank
        var asrank = parseInt(match.rank);        // as 'vsrank'
        if ('Top 5' in mults && vsrank < 6) {return 1;}
        else if ('Top 10' in mults && vsrank < 11) {return 1;}
        else if ('Top 20' in mults && vsrank < 21) {return 1;}
        else if ('Top 50' in mults && vsrank < 51) {return 1;}
        else if ('Top 100' in mults && vsrank < 101) {return 1;}
        else if ('11+' in mults && vsrank > 10) {return 1;}
        else if ('21+' in mults && vsrank > 20) {return 1;}
        else if ('51+' in mults && vsrank > 50) {return 1;}
        else if ('101+' in mults && vsrank > 100) {return 1;}
        else if ('Higher' in mults && (vsrank < asrank)) {return 1;}
        else if ('Lower' in mults && (vsrank > asrank)) {return 1;}
        else if ('Custom' in mults) {
            if (vsrank >= lowrank && vsrank <= highrank) {return 1;}
            return 0;
            }
        else {return 0;}
        }
    else if (fid == 'crank') {
        if (!(match.opp in currRank)) {
            if ('Inactive' in mults) {return 1;}
            else {return 0;} // disallows, say, 'Retired' + 'Top 10' -- not strictly correct    
            }  
        else if ('Top 10' in mults && parseInt(currRank[match.opp]) < 11) {return 1;}
        else if ('Top 20' in mults && parseInt(currRank[match.opp]) < 21) {return 1;}
        else if ('Top 50' in mults && parseInt(currRank[match.opp]) < 51) {return 1;}
        else if ('Top 100' in mults && parseInt(currRank[match.opp]) < 101) {return 1;}
        else if ('Active' in mults) {return 1;}
        else {return 0;}
        }        
    else if (fid == 'asrank') {
        if (match.rank == 'UNR') {match.rank = 2000;}
        if ('Number 1' in mults&& parseInt(match.rank) <= 1) {return 1;}
        if ('Top 5' in mults && parseInt(match.rank) <= 5) {return 1;}
        if ('Top 10' in mults && parseInt(match.rank) <= 10) {return 1;}
        else if ('Top 20' in mults && parseInt(match.rank) <= 20) {return 1;}
        else if ('Top 50' in mults && parseInt(match.rank) <= 50) {return 1;}
        else if ('Top 100' in mults && parseInt(match.rank) <= 100) {return 1;}
        else if ('Top 200' in mults && parseInt(match.rank) <= 200) {return 1;}
        else if ('2+' in mults && parseInt(match.rank) >= 2) {return 1;}
        else if ('6+' in mults && parseInt(match.rank) >= 6) {return 1;}
        else if ('11+' in mults && parseInt(match.rank) >= 11) {return 1;}
        else if ('21+' in mults && parseInt(match.rank) >= 21) {return 1;}
        else if ('51+' in mults && parseInt(match.rank) >= 51) {return 1;}
        else if ('101+' in mults && parseInt(match.rank) >= 101) {return 1;}
        else if ('201+' in mults && parseInt(match.rank) >= 201) {return 1;}
        else if ('Custom' in mults) {
            if (parseInt(match.rank) >= lowrank && parseInt(match.rank) <= highrank) {return 1;}
            return 0;
            }
        else {return 0;}
        }
    else if (fid == 'level') {
        if ('All ATP' in mults) {
            if (match.level == 'Q' || match.level == 'C') {}
            else if (match.level == 'J' || match.level == 'S') {}
            else if (match.level == '15' || match.level == '25') {}
            else if (match.round == 'Q1' || match.round == 'Q2') {} // redundant, but current week
            else if (match.round == 'Q3' || match.round == 'Q4') {} // qualies 'level' aren't correctly labeled
            else {return 1;} 
            }
        if ('All Pro' in mults) {
            if (match.level == 'J') {}
            else {return 1;}
            }
        if ('Challengers' in mults) {
            if (match.level != 'C') {}
            else if (match.round == 'Q1' || match.round == 'Q2') {} // redundant, but current week
            else if (match.round == 'Q3' || match.round == 'Q4') {} // qualies 'level' aren't correctly labeled
            else {return 1;} 
            }
        if ('CH Qualies' in mults) {
            if (match.level != 'C') {}
            else if (match.round.slice(0,1) == 'R' || match.round == 'QF') {} // redundant, but current week
            else if (match.round == 'SF' || match.round == 'F') {} // qualies 'level' aren't correctly labeled
            else {return 1;} 
            }
        if (levdict2[match.level] in mults) {return 1;}
        else {return 0;}
        }
    else if (fid == 'round') {
        if (rddict2[match.round] in mults) {return 1;}
        if ('First Round' in mults && parseInt(match.roundnum) == 1) {return 1;}
        if ('Second Round' in mults && parseInt(match.roundnum) == 2) {return 1;}
        if ('Third Round' in mults && parseInt(match.roundnum) == 3) {return 1;}
        if ('First Match' in mults && parseInt(match.matchnum) == 1) {return 1;}
        if ('Second Match' in mults && parseInt(match.matchnum) == 2) {return 1;}
        if ('Third Match' in mults && parseInt(match.matchnum) == 3) {return 1;}
        else {return 0;}
        }
    else if (fid == 'span') {
        if ('Career' in mults) {return 1;}
        else if ('Last 52' in mults) {
            var keyday;
            if (doubles == 1) {
                if (active_dubs == 1) {keyday = today;}
                else {keyday = lastdate_dubs;}
                }
            else {
                if (active == 1) {keyday = today;}
                else {keyday = lastdate;}
                }
            if (parseInt(match.date) < (keyday-10000)) {}
            //else if (parseInt(match.date) > keyday) {}
            else {return 1;}
            }
        if ('Custom' in mults) {
            if (parseInt(match.date) >= startdate && parseInt(match.date) <= enddate) {return 1;}
            return 0;
            }
        var yr = match.date.slice(0,4);
        var mmdd = match.date.slice(4);
        var syr;
        if (parseInt(mmdd) > 1226) {
            syr = (parseInt(yr) + 1) + '';
            }
        else {syr = yr;}
        if (syr in mults) {return 1;}
        else {return 0;}
        }
    else if (fid == 'entry') {  // -1 because entry can be '' for seeds
        if ('Qualifier' in mults && (match.entry == 'Q' || match.entry == 'LL')) {return 1;}
        else if ('Wild Card' in mults && match.entry == 'WC') {return 1;}
        else if ('Seeded' in mults && match.seed.length > 0) {return 1;}
        else if (match.level == 'F' || match.level == 'D') {return 0;}
        else if ('Unseeded' in mults && match.seed.length == 0) {return 1;}
        else {return 0;}
        }
    else if (fid == 'oentry') {  // -1 because entry can be '' for seeds
        if ('Qualifier' in mults && (match.oentry == 'Q' || match.oentry == 'LL')) {return 1;}
        else if ('Wild Card' in mults && match.oentry == 'WC') {return 1;}
        else if ('Seeded' in mults && match.oseed.length > 0) {return 1;}
        else if (match.level == 'F' || match.level == 'D') {return 0;}
        else if ('Unseeded' in mults && match.oseed.length == 0) {return 1;}
        else {return 0;}
        }
    else if (fid == 'height') {  
        if ('Shorter' in mults && parseInt(match.oht) < ht) {return 1;}
        else if ('Taller' in mults && parseInt(match.oht) > ht) {return 1;}
        else if ("Under 5'10" in mults && parseInt(match.oht) < 178) {return 1;}
        else if ("Under 6'0" in mults && parseInt(match.oht) < 183) {return 1;}
        else if ("Over 6'2" in mults && parseInt(match.oht) > 188) {return 1;}
        else if ("Over 6'4" in mults && parseInt(match.oht) > 193) {return 1;}
        else {return 0;}
        }
    else if (fid == 'sets') {
        if (match.score == '') {return 0;}
        if (match.score == 'W/O') {return 0;}
        if ('All Best of 3' in mults && (parseInt(match.max) == 3)) {return 1;}
        else if ('All Best of 5' in mults && (parseInt(match.max) == 5)) {return 1;}
        if ('Straights' in mults || 'Deciders' in mults) {
            //if (match.score == 'W/O') {return 0;} // only because later filters could not possibly include this
            var sets = match.score.split(' ');
            var nsets = sets.length;
            if (sets[(sets.length-1)] == 'RET') {nsets = nsets - 1;}
            if ('Straights' in mults && (match.max/nsets >= 1.4)) {return 1;}
            else if ('Deciders' in mults && (parseInt(match.max) == nsets)) {return 1;}
            }
        if ('5-Setters' in mults && (parseInt(match.max) == 5)) {
            var sets = match.score.split(' ');
            var nsets = sets.length;
            if (sets[(sets.length-1)] == 'RET') {nsets = nsets - 1;}
            if (nsets == 5) {return 1;}
            }
        if ('4-Setters' in mults && (parseInt(match.max) == 5)) {
            var sets = match.score.split(' ');
            var nsets = sets.length;
            if (sets[(sets.length-1)] == 'RET') {nsets = nsets - 1;}
            if (nsets == 4) {return 1;}
            }
        if ('3 Sets (of 5)' in mults && (parseInt(match.max) == 5)) {
            var sets = match.score.split(' ');
            var nsets = sets.length;
            if (sets[(sets.length-1)] == 'RET') {nsets = nsets - 1;}
            if (nsets == 3) {return 1;}
            }
        if ('3 Sets (of 3)' in mults && (parseInt(match.max) == 3)) {
            var sets = match.score.split(' ');
            var nsets = sets.length;
            if (sets[(sets.length-1)] == 'RET') {nsets = nsets - 1;}
            if (nsets == 3) {return 1;}
            }
        if ('2-Setters' in mults && (parseInt(match.max) == 3)) {
            var sets = match.score.split(' ');
            var nsets = sets.length;
            if (sets[(sets.length-1)] == 'RET') {nsets = nsets - 1;}
            if (nsets == 2) {return 1;}
            }
        }
    else if (fid == 'scores') {
        if (match.score.indexOf('7-6') != -1) {
            if ('All tiebreaks' in mults) {return 1;}
            else if ('TB won' in mults && match.wl == 'W') {return 1;}
            else if ('TB lost' in mults && match.wl == 'L') {return 1;}
            }
        if (match.score.indexOf('6-7') != -1) {
            if ('All tiebreaks' in mults) {return 1;}
            else if ('TB won' in mults && match.wl == 'L') {return 1;}
            else if ('TB lost' in mults && match.wl == 'W') {return 1;}
            }
        if ('Deciding TB' in mults) {
            var sets = match.score.split(' ');
            var nsets = sets.length;
            if (sets[(sets.length-1)] == 'RET') {nsets = nsets - 1;}
            if (parseInt(match.max) == nsets) {
                var lastset = sets[sets.length-1];
                if (lastset.indexOf('7-6') != -1) {return 1;}
                else if (lastset.indexOf('6-7') != -1) {return 1;}
                }     
            }
        if (match.score.indexOf('7-5') != -1) {
            if ('All 7-5' in mults) {return 1;}
            else if ('7-5 won' in mults && match.wl == 'W') {return 1;}
            else if ('7-5 lost' in mults && match.wl == 'L') {return 1;}
            }
        if (match.score.indexOf('5-7') != -1) {
            if ('All 7-5' in mults) {return 1;}
            else if ('7-5 won' in mults && match.wl == 'L') {return 1;}
            else if ('7-5 lost' in mults && match.wl == 'W') {return 1;}
            }  
        if (match.score.indexOf('6-0') != -1) {
            if ('All bagels' in mults) {return 1;}
            else if ('6-0 won' in mults && match.wl == 'W') {return 1;}
            else if ('6-0 lost' in mults && match.wl == 'L') {return 1;}
            }
        if (match.score.indexOf('0-6') != -1) {
            if (match.score.indexOf('70-68') != -1) {return 0;} 
            else if ('All bagels' in mults) {return 1;}
            else if ('6-0 won' in mults && match.wl == 'L') {return 1;}
            else if ('6-0 lost' in mults && match.wl == 'W') {return 1;}
            }  
        if (match.score.indexOf('6-1') != -1) {
            if ('All 6-1' in mults) {return 1;}
            else if ('6-1 won' in mults && match.wl == 'W') {return 1;}
            else if ('6-1 lost' in mults && match.wl == 'L') {return 1;}
            }
        if (match.score.indexOf('1-6') != -1) {
            if ('All 6-1' in mults) {return 1;}
            else if ('6-1 won' in mults && match.wl == 'L') {return 1;}
            else if ('6-1 lost' in mults && match.wl == 'W') {return 1;}
            } 
        return 0;                    
        }
    else if (fid == 'results') {
        if (match.score == '') {return 0;}
        if (match.score.indexOf('W') != -1 || match.score.indexOf('w') != -1) {
            if ('W by W/O' in mults && match.wl == 'W') {return 1;}
            else if ('L by W/O' in mults && match.wl == 'L') {return 1;}
            else {return 0;}
            } // only because later filters could not possibly include this
        else if (match.score.slice(-3) == 'RET') {
            if ('W by RET' in mults && match.wl == 'W') {return 1;}
            else if ('Wins' in mults && match.wl == 'W') {return 1;}
            else if ('L by RET' in mults && match.wl == 'L') {return 1;}
            else if ('Losses' in mults && match.wl == 'L') {return 1;}
            else {return 0;}
            }  
        else if (match.score.slice(-3) == 'DEF' || match.score.slice(-3) == 'ABD') {
            if ('Completed' in mults) {return 0;}
            }     
        else if ('Completed' in mults) {return 1;}
        if ('Wins' in mults && match.wl == 'W') {return 1;}
        else if ('Losses' in mults && match.wl == 'L') {return 1;}
        var sets = match.score.split(' ');
        if (sets[1] == 'RET') {return 0;} // only because this is the last filter
        var firstset = sets[0];
        var a = parseInt(firstset[0]), b = parseInt(firstset[2]);
        var wonfirst;
        if (match.wl == 'W') {
            if (a>b) {wonfirst=1;}
            else {wonfirst=0;}
            }
        else {
            if (a>b) {wonfirst=0;}
            else {wonfirst=1;}
            }
        if ('Won 1st Set' in mults && wonfirst == 1) {return 1;}
        else if ('Lost 1st Set' in mults && wonfirst == 0) {return 1;}
        if (sets.length < 3) {return 0;} // limit to five-setters
        else if (sets[2] == 'RET') {return 0;} // only because this is the last filter
        var secondset = sets[1];
        var a = parseInt(secondset[0]), b = parseInt(secondset[2]);
        var wonsecond;
        if (match.wl == 'W') {
            if (a>b) {wonsecond=1;}
            else {wonsecond=0;}
            }
        else {
            if (a>b) {wonsecond=0;}
            else {wonsecond=1;}
            }
        if ('Won Sets 1&2' in mults && (wonfirst == 1 && wonsecond == 1)) {return 1;}
        else if ('Lost Sets 1&2' in mults && (wonsecond == 0 && wonfirst == 0)) {return 1;}
        else if ('Split 1&2' in mults && (wonfirst != wonsecond)) {return 1;}
        if (sets.length < 4) {return 0;} // limit to four-plus setters
        else if (sets[3] == 'RET') {return 0;} // only because this is the last filter
        var thirdset = sets[2];
        var a = parseInt(thirdset[0]), b = parseInt(thirdset[2]);
        var wonthird;
        if (match.wl == 'W') {
            if (a>b) {wonthird=1;}
            else {wonthird=0;}
            }
        else {
            if (a>b) {wonthird=0;}
            else {wonthird=1;}
            }
        var setswon = wonfirst+wonsecond+wonthird
        if ('Up 2-1 Sets' in mults && setswon == 2) {return 1;}
        else if ('Down 1\u20112 Sets' in mults && setswon == 1) {return 1;}        
        return 0;
        }
    else if (fid == 'tourney') {
        if ('Tour Finals' in mults) {
            if (match.tourn == 'Tour Finals' || match.tourn == 'Masters Cup') {return 1;}
            }
        else if ('Olympics' in mults) {
            if (match.tourn.indexOf('Olympics') != -1) {return 1;}
            }
        else if ('Davis Cup' in mults) {
            if (match.tourn.indexOf('Davis Cup') != -1) {return 1;}
            }
        else if (match.tourn.slice(-2) == ' Q') {
            if (match.tourn.slice(0,-2) in mults) {return 1;}
            }
        if (match.tourn in mults) {return 1;}
        else if (match.tourn.slice(4) in mults) {return 1;}
        return 0;
        }
    else if (fid == 'country') {
        if (match.ocountry in mults) {return 1;}
        return 0;
        }
    else if (fid == 'surface') {
        if (match.surf in mults) {return 1;}
        return 0;
        }
    else if (fid == 'h2h') {
        if (match.opp in mults) {return 1;}
        return 0;
        }
    else if (fid == 'not') {
        if (match.opp in mults) {return 0;}
        return 1;
        }
    else if (fid == 'partner') {
        if (match.partner in mults) {return 1;}
        return 0;        
        }
    else if (fid == 'opp') {
        if (match.opp in mults) {return 1;}
        else if (match.opp2 in mults) {return 1;}
        return 0;        
        }
    else if (fid == 'oppteam') {
        var fullteam = match.opp + '/' + match.opp2;
        if (fullteam in mults) {return 1;}
        return 0;        
        }    
    else if (fid == 'stats') {
        //alert('in stats filter');
        // eventually validate / check that choice and operator are not defaults
        var thresh = Number(statinput);
        if (isNaN(thresh) == true) {return 1;} // some kind of message?
        // get relevant stat for this match; eventually put this in an external function
        // if no stats, return 0 ?
        var mstat = 0;
        if (statchoice == 'Dom Ratio') {
            if (match.ofwon == "") {return 0;}
            var rpw = 1 - (parseInt(match.ofwon) + parseInt(match.oswon))/match.opts; // dominance ratio:
            var spl = 1 - ((parseInt(match.fwon) + parseInt(match.swon))/match.pts);
            mstat = rpw/spl;
            }
        else if (statchoice == 'Ace Perc') {
            if (match.aces == "") {return 0;}
            mstat = (match.aces/match.pts)*100;
            }
        else if (statchoice == 'DF Perc') {
            if (match.dfs == "") {return 0;}
            mstat = (match.dfs/match.pts)*100;
            }
        else if (statchoice == '1st In') {
            if (match.firsts == "") {return 0;}
            mstat = (match.firsts/match.pts)*100;
            }
        else if (statchoice == '1st WPc') {
            if (match.fwon == "") {return 0;}
            mstat = (match.fwon/match.firsts)*100;
            }
        else if (statchoice == '2nd WPc') {
            if (match.swon == "") {return 0;}
            mstat = (match.swon/(match.pts-match.firsts))*100;
            }
        else if (statchoice == 'BP Svd Pc') {
            if (match.saved == "") {return 0;}
            mstat = (match.saved/match.chances)*100;
            }
        else if (statchoice == 'BP Saved') {
            if (match.saved == "") {return 0;}
            mstat = parseInt(match.saved);
            }
        else if (statchoice == 'BPC Faced') {
            if (match.chances == "") {return 0;}
            mstat = parseInt(match.chances);
            }
        else if (statchoice == 'TPW') {
            if (match.fwon == "") {return 0;}
            var pointswon = parseInt(match.fwon) + parseInt(match.swon) + (match.opts - match.ofwon - match.oswon);
            mstat = (pointswon/(parseInt(match.pts) + parseInt(match.opts)))*100;
            }
        else if (statchoice == 'RPW') {
            if (match.ofwon == "") {return 0;}
            mstat = (1 - ((parseInt(match.ofwon) + parseInt(match.oswon))/match.opts))*100;
            }
        else if (statchoice == 'vAce Pc') {
            if (match.oaces == "") {return 0;}
            mstat = (match.oaces/match.opts)*100;
            }
        else if (statchoice == 'v1st WPc') {
            if (match.ofwon == "") {return 0;}
            mstat = (1 - (match.ofwon/match.ofirsts))*100;
            }
        else if (statchoice == 'v2nd WPc') {
            if (match.oswon == "") {return 0;}
            mstat = (1 - (match.oswon/(match.opts-match.ofirsts)))*100;
            }
        else if (statchoice == 'BP Cnv Pc') {
            if (match.osaved == "") {return 0;}
            mstat = (1 - (match.osaved/match.ochances))*100;
            }
        else if (statchoice == 'BP Conv') {
            if (match.ochances == "") {return 0;}
            mstat = (match.ochances - match.osaved);
            }
        else if (statchoice == 'BP Chncs') {
            if (match.ochances == "") {return 0;}
            mstat = parseInt(match.ochances);
            }
        else if (statchoice == 'Tot Pts') {
            if (match.pts == "") {return 0;}
            mstat = (parseInt(match.pts) + parseInt(match.opts));
            }
        else if (statchoice == 'Aces') {
            if (match.aces == "") {return 0;}
            mstat = parseInt(match.aces);
            }
        else if (statchoice == 'DFs') {
            if (match.dfs == "") {return 0;}
            mstat = parseInt(match.dfs);
            }
        else if (statchoice == 'Sv Pts') {
            if (match.pts == "") {return 0;}
            mstat = parseInt(match.pts);
            }
        else if (statchoice == '1Sv Pts') {
            if (match.firsts == "") {return 0;}
            mstat = parseInt(match.firsts);
            }
        else if (statchoice == '2Sv Pts') {
            if (match.pts == "") {return 0;}
            mstat = match.pts-match.firsts;
            }
        else if (statchoice == 'vAces') {
            if (match.oaces == "") {return 0;}
            mstat = parseInt(match.oaces);
            }
        else if (statchoice == 'Time') {
            if (match.time == "") {return 0;}
            mstat = parseInt(match.time);
            }            
        else {return 1;} // invalid stat choice; some kind of message?
        // separate based on operator
        if (statoperatorchoice == 'gt' && mstat <= thresh) {return 0;}
        else if (statoperatorchoice == 'lt' && mstat >= thresh) {return 0;}
        else if (statoperatorchoice == 'ge' && mstat < thresh) {return 0;}
        else if (statoperatorchoice == 'le' && mstat > thresh) {return 0;}
        else if (statoperatorchoice == 'eq' && mstat != thresh) {return 0;}
        else {return 1;} // invalid operator; some kind of message?
        }
    }
    
function unitePartners(multselect) {
    // filter displays doubles team on two lines,
    // so filter parser thinks that's two items, not 
    // one. this function glues them back together
    var mults = {};
    var mult_list = [];
    for (var i=0; i<multselect.length; i++) {
        if (i % 2 == 0) {
            var team = multselect[i];
            }
        else {
            team = team + multselect[i].slice(1);
            mults[team.replace(/\u00a0/g, ' ')] = 1;
            mult_list.push(team.replace(/\u00a0/g, ' '));
            }
        }  
    return [mults, mult_list];  
    }

function genfilter(fid, first, match, att) {
    // fid = filter id; first = default select; match = relevant match object; att = rel attribute
    // find e.g. 'surfaceselected' text -- that's it
    var selname = '.' + fid + 'selected';
    var multselect = $(selname).text().split(fourspaces + fourspaces).slice(1); // list of selected for this filter
    if (multselect[0] == undefined) { // ugly IE hack
    	var multselect = $(selname).text().split("        ").slice(1);
    	}
    if (fid == "oppteam" && multselect[0] != "All") {
        var mult_unit = unitePartners(multselect);
        var mults = mult_unit[0];
        }
    else {
        var mults = {};
        // make list into object, for 'in' searching
        for (var i=0; i<multselect.length; i++) {mults[multselect[i].replace(/\u00a0/g, ' ')] = 1;}  
        }
    if ('All' in mults) {return 1;}
    else if (filterDict(fid, match, mults) != 1) {return 0;} // this is where it gets messy
    return 1;
    }

function hidePermalink() {
    if ($(".perma").text() == 'Hide Permalink') { // generalize this, also add to other toggles that change permalink?
        $(".perma").html('Table Permalink');
        $("#permalink").remove();
        }
    }

function getWinLossTiebreak(match) {
    var tb = 0, tbwon = 0;
    if (match.score.search('W/O') == -1 && match.score != '') {
        if (match.wl == 'W') {match["wins"] = 1; match["losses"] = 0;}
        else {match["losses"] = 1; match["wins"] = 0;}
        }
    else {match["wins"] = 0; match["losses"] = 0;}
    // do some calculations
    sets = match.score.split(' ');       
    for (var i=0; i<sets.length; i++) {
        // tb counting
        if (sets[i].slice(0, 3) == '7-6' || sets[i] == '13-12') {
            tb++;
            if (match.wl == 'W') {tbwon++;}
            }
        else if (sets[i].slice(0, 3) == '6-7') {
            tb++;
            if (match.wl == 'L') {tbwon++;}
            }
        }
    match["tiebreaks"] = tb;
    match["tbwon"] = tbwon;
    return match;
    }

function makeSplitStatRow(mt) {
    var wl = mt["wins"] + '-' + mt["losses"] + ' (' + alignRound((mt["wins"]/(mt['wins'] + mt['losses'])), 0, 1) + ')';
    var tbrecord = mt["tbwon"] + '-' + (mt["tiebreaks"]-mt["tbwon"]) + ' (' + alignRound((mt["tbwon"]/(mt["tiebreaks"])), 0, 1) + ')';
    var acerate = alignRound((mt["aces"]/mt["pts"]), 1, 1);
    var firstin = alignRound((mt["firsts"]/mt["pts"]), 1, 1);
    var fwin = alignRound((mt.fwon/mt.firsts), 1, 1);
    var swin = alignRound((mt.swon/(mt.pts-mt.firsts)), 1, 1);
    var rpw = 1 - (mt.ofwon + mt.oswon)/mt.opts;
    var rpwShow = alignRound(rpw, 1, 1)
    var spw = (mt.fwon + mt.swon)/mt.pts;
    var spwShow = alignRound(spw, 1, 1)
    var spl = 1 - spw;
    var tpw = (mt.fwon + mt.swon + mt.opts - mt.ofwon - mt.oswon)/(mt.pts + mt.opts);
    var tpwShow = alignRound(tpw, 1, 1)
    var domratio = alignRound(rpw/spl, 2);
    var holds = mt.games - (mt.chances - mt.saved);
    var hld = holds / mt.games;
    var hldShow = alignRound(hld, 1, 1);
    var breaks = mt.ochances - mt.osaved;
    var brk = breaks / mt.ogames;
    var brkShow = alignRound(brk, 1, 1);
    var statrow = [wl, tbrecord, acerate, firstin, fwin, swin, hldShow, spwShow, brkShow, rpwShow, tpwShow, domratio];
    return statrow;
    }

var matchhead = ["date","tourn","surf","level","wl","rank","seed","entry","round",
                 "score","max","opp","orank","oseed","oentry","ohand","obday",
                 "oht","ocountry","oactive","time","aces","dfs","pts","firsts","fwon",
                 "swon",'games',"saved","chances","oaces","odfs","opts","ofirsts",
                 "ofwon","oswon",'ogames',"osaved","ochances", "obackhand", "chartlink",
                 "pslink","whserver","matchid","wh","roundnum","matchnum"]
                 
var matchhead_dubs = ["date","tourn","surf","level","wl","rank","seed","entry","round",
                 "score","max","partner", "partnerlast", "prank", "phand", "pbday", "pht", "pcountry", "pactive",
              "oseed", "oentry", "opp","olast","orank","ohand","obday","oht","ocountry","oactive",
              "opp2","o2last","o2rank","o2hand","o2bday","o2ht","o2country","o2active",
              "time","aces","dfs","pts","firsts","fwon",
                 "swon",'games',"saved","chances","oaces","odfs","opts","ofirsts",
                 "ofwon","oswon",'ogames',"osaved","ochances", "obackhand", "chartlink",
                 "pslink","whserver","matchid","wh","roundnum","matchnum"]

var totals = {"aces": 0,"pts": 0,"firsts": 0,"fwon": 0, "swon": 0, "oaces": 0,"opts": 0,"ofirsts":0,"ofwon": 0,"oswon": 0,
              "wins":0, "losses":0, "tiebreaks":0, "tbwon":0, "games":0, "ogames":0, "saved":0, "osaved":0, "ochances":0, "chances":0};
//var stats = ["aces","pts","firsts","fwon", "swon", "oaces","opts","ofirsts","ofwon","oswon", "wins","losses", "tiebreaks", "tbwon"];
// extending this for h2h tally purposes
var stats = ["aces","dfs","pts","firsts","fwon", "swon", "saved", "chances", "oaces","opts", "ofirsts","ofwon","oswon", 
             "osaved","ochances","wins","losses", "tiebreaks", "tbwon", "games", "ogames"];

function addYearSplits() {
    if (!$('#tabDubs').hasClass("tablink")) {
        var splits_head = matchhead_dubs;
        var splits_matches = matchmx_dubs;
        }
    else {
        var splits_head = matchhead;
        var splits_matches = matchmx;
        }
    // remove/set aside bottom row?! need id tag for bottom row
    var splitlist = [];
    var splits = {'Career': $.extend(true, {}, totals)};
    for (m=0; m<splits_matches.length; m++) {
        var match = {}
        for (var x=0; x<splits_head.length; x++) {
            if (splits_matches[m].length <= x) {match[splits_head[x]] = '';}
            else {match[splits_head[x]] = splits_matches[m][x];}
            } 
        var msplits = ['Career'];  
        //var match = matches[m];
        if (parseInt(match.date.slice(4)) > 1224) {var year = (parseInt(match.date.slice(0,4)) + 1) + '';}
        else {var year = match.date.slice(0,4);}
        if (year in splits) {}
        else {
            splits[year] = $.extend(true, {}, totals);
            splitlist.push(year);
            }
        msplits.push(year);
        match = getWinLossTiebreak(match);   
        for (u=0; u<stats.length; u++) {
            t = stats[u];
            for (i=0; i<msplits.length; i++) {
                if (match[t] != '') {splits[msplits[i]][t] = splits[msplits[i]][t] + parseInt(match[t]);}
                }
            }
        }
    splitlist.sort();
    splitlist.reverse();
    splitlist.push('Career');
    for (j=0; j<splitlist.length; j++) {
        var idnum = j+1;
        if (j == (splitlist.length-1)) {idnum = 0;}
        var yearid = 'sA' + idnum;
        var $row = $('<tr>').attr('class', 'yearsplits').attr('id', yearid);
        var split = splitlist[j], mt = splits[split];
        var statrow = makeSplitStatRow(mt);
        var $splitspan = $('<span>').text(split + ' ');
        //var $splitclick = $('<span>').text('(+)').attr('class', 'likelink splitclick');
        $row.append($('<td>').append($splitspan)); //.append($splitclick)); 
        var cells = [];
        for (var i=0; i < statrow.length; i++) {           
            $row.append($('<td>').text(statrow[i]).attr('align', 'right'));
            }
        $("#overall").append($row);
        }
    }

var splitIds = {'Last 52': '',
                'Hard': 'B0',
                'Clay': 'B1',
                'Grass': 'B2',
                'Grand Slams': 'C0',
                'vs Top 10': 'I0',
                'vs Righties': 'K0',
                'vs Lefties': 'K1',
                'Best of 3': 'F2',
                'Best of 5': 'F3',
                'Deciding Sets': 'F5'
                }

function applyPrefilters() {
    if (prefilters.length != 0) {
        for (j=0; j<filterlist.length; j++) {
            var filter = filterlist[j];
            if (filter in prefilters) {
                $("." + filter + "choice").removeClass("selected").removeClass(filter + "selected");
                if (filter in alphaFilters) {
                    var applylist = prefilters[filter].split(',');
                    }
                else if (filter == 'stats') {
                    var applylist = [prefilters[filter]]; // will break if ever multiple stat params
                    }
                else {
                    var applylist = prefilters[filter].split('i');
                    }
                for (k=0; k<applylist.length; k++) {
                    // treat spanCustom separately
                    if (applylist[k].slice(0,2) == 'cx' && filter != "stats") {
                        $("#" + filter + 'Customqq').addClass("selected").addClass(filter + "selected");
                        if (filter == 'span') {
                            startdate = applylist[k].slice(2,10);
                            enddate = applylist[k].slice(10,18);
                            }
                        else if (filter == 'rank') {
                            lowrank = parseInt(applylist[k].slice(2,7)) - 10000;
                            highrank = parseInt(applylist[k].slice(7,12)) - 10000;
                            }
                        }
                    else if (applylist[k].slice(0,2) == 'cx' && filter == "stats") {  
                        $("#" + filter + '0').addClass("selected").addClass(filter + "selected");
                        var statparamstring = applylist[k].slice(2,-2);
                        var statparams = statparamstring.split('-');
                        statchoice = statparams[0].replace(/_/g," ");
                        statoperatorchoice = statparams[1];
                        statinput = statparams[2];
                        }                    
                    else {              
                        $("#" + filter + applylist[k]).addClass("selected").addClass(filter + "selected");
                        }
                    if ($("#" + filter + "head").hasClass('closed')) {$("#" + filter + "head").addClass("selected");}
                    }
                }
            }
        }
    if (opponent.length != 0) {
        var opps = opponent.split(',');
        for (i=0; i<opps.length; i++) {
            var player = insertNameSpaces(opps[i]);
            setPlayerFilter(player, 0, 0);
            setOpponentFilter(player, 0, 0);
            }
        for (i=0; i<opps.length; i++) {
            var playerid = '#h2h' + opps[i];
            $(playerid).addClass('selected').addClass('h2hselected');
            var playerid2 = '#opp' + opps[i];
            $(playerid2).addClass('selected').addClass('oppselected');
            }
        $("#h2hhead").addClass("selected");
        $("#opphead").addClass("selected");
        }
    else {
        $(".h2hchoice").removeClass("selected").removeClass("h2hselected");
        $("#h2hdef").addClass("selected").addClass("h2hselected");
        $(".oppchoice").removeClass("selected").removeClass("oppselected");
        $("#oppdef").addClass("selected").addClass("oppselected");
        }
    if (exclude.length != 0) {
        var nots = exclude.split(',');
        for (i=0; i<nots.length; i++) {
            var player = insertNameSpaces(nots[i]);
            setNotPlayerFilter(player, 0);
            }
        for (i=0; i<nots.length; i++) {
            var playerid = '#not' + nots[i];
            $(playerid).addClass('selected').addClass('notselected');
            }
        $("#nothead").addClass("selected");
        }
    else {
        $(".notchoice").removeClass("selected").removeClass("notselected");
        $("#notdef").addClass("selected").addClass("notselected");
        //$(".notchoice").toggle();
        }
    if (partner.length != 0) {
        var partners = partner.split(',');
        for (i=0; i<partners.length; i++) {
            var player = insertNameSpaces(partners[i]);
            setPartnerFilter(player, 0);
            }
        for (i=0; i<partners.length; i++) {
            var playerid = '#partner' + partners[i];
            $(playerid).addClass('selected').addClass('partnerselected');
            }
        $("#partnerhead").addClass("selected");
        }
    else {
        $(".partnerchoice").removeClass("selected").removeClass("partnerselected");
        $("#partnerdef").addClass("selected").addClass("partnerselected");
        }
    if (opp_team.length != 0) {
        var teams = opp_team.split(',');
        for (i=0; i<teams.length; i++) {
            var player = insertNameSpaces(teams[i]);
            setOppteamFilter(player, 0);
            }
        for (i=0; i<teams.length; i++) {
            var playerid = '#oppteam' + teams[i];
            $(playerid).addClass('selected').addClass('oppteamselected');
            }
        $("#oppteamhead").addClass("selected");
        }
    else {
        $(".oppteamchoice").removeClass("selected").removeClass("oppteamselected");
        $("#oppteamdef").addClass("selected").addClass("oppteamselected");
        }
    $(".tabview").addClass('tablink');
    if (view.length != 0) {
        if (view == "h2h") {
            $("#tabHead").removeClass('tablink');
            $(".header").show();
            $("#h2hhead").hide();
            $("#nothead").hide();
            $("#minyearshead").hide(); 
            $(".minyearschoice").hide();
            $("#partnerhead").hide();
            $(".partnerchoice").hide();
            $("#opphead").hide();
            $(".oppchoice").hide();
            $("#oppteamhead").hide();
            $(".oppteamchoice").hide();
            $("#handshead").hide();
            $(".handschoice").hide();    
            $("#prankhead").hide();
            $(".prankchoice").hide();
            $("#phandhead").hide();
            $(".phandchoice").hide();        
            }
        else if (view == "events") {
            $("#tabEvents").removeClass('tablink');
            $(".header").show();
            $("#h2hhead").hide();
            $("#nothead").hide();  
            $("#partnerhead").hide();
            $(".partnerchoice").hide();
            $("#opphead").hide();
            $(".oppchoice").hide();
            $("#oppteamhead").hide();
            $(".oppteamchoice").hide();
            $("#handshead").hide();
            $(".handschoice").hide();   
            $("#prankhead").hide();
            $(".prankchoice").hide();
            $("#phandhead").hide();
            $(".phandchoice").hide();        
            }
        else if (view == "doubles") {
            $("#tabEvents").removeClass('tablink');
            $(".header").show();
            $("#h2hhead").hide();
            $("#nothead").hide(); 
            $("#minimumhead").hide();
            $(".minimumchoice").hide();
            $("#minyearshead").hide();
            $(".minyearschoice").hide();
            $("#crankhead").hide();
            $(".crankchoice").hide();
            $("#handhead").hide();
            $(".handchoice").hide();	
            $("#agehead").hide();
            $(".agechoice").hide();
            $("#heighthead").hide();
            $(".heightchoice").hide();
            $("#countryhead").hide();
            $(".countrychoice").hide();
            }
        }
    else { // if view == '', set to results
	      $("#tabResults").removeClass('tablink');
        $(".header").show();
        $("#minimumhead").hide();
        $(".minimumchoice").hide();
        $("#minyearshead").hide();
        $(".minyearschoice").hide();
        $("#partnerhead").hide();
        $(".partnerchoice").hide();
        $("#opphead").hide();
        $(".oppchoice").hide();
        $("#oppteamhead").hide();
        $(".oppteamchoice").hide();
        $("#handshead").hide();
        $(".handschoice").hide();    
        $("#prankhead").hide();
        $(".prankchoice").hide();
        $("#phandhead").hide();
        $(".phandchoice").hide();    	
       } 
    }

var spelled = {'A': '', 'D': '', 'N': '', 'I': '', 'T': ''};
function newPrefilters(filterstring) {
    //alert(filterstring);
    var io = filterstring.indexOf('&q=');
    if (io == -1) {opponent = '';}
    else {
        opponent = filterstring.slice(io+3);
        filterstring = filterstring.slice(0, io);
        }
    var xo = filterstring.indexOf('&x=');
    if (xo == -1) {exclude = '';}
    else {
        exclude = filterstring.slice(xo+3);
        filterstring = filterstring.slice(0, xo);
        }
    var vo = filterstring.indexOf('&view=');
    if (vo == -1) {view = '';}
    else {
        view = filterstring.slice(vo+6);
        filterstring = filterstring.slice(0, vo);
        }
    //alert(filterstring);
    prefilters = {};
    filterstring += 'Z';
    var start = 0;
    if (filterstring[0] in spelled) {var go = 0;}
    else {var go = 1;}
    for (c=1; c<filterstring.length; c++) {
        if (filterstring[c] == 'q' && filterstring[c-1] == 'q') {
            if ((c+1)<filterstring.length && filterstring[c+1] == ',') {}
            else {go = 1;}
            }
        else if (filterstring[c] in {'0': 1, '1': 1, '2': 1, '3': 1, '4': 1, '5': 1, '6': 1, '7': 1, '8': 1, '9': 1, 'i': 1, ',': 1, '+': 1} && go == 1) {}
        else if (go == 1) {
            var chunk = filterstring.slice(start, c);
            var k = url2filter[chunk[0]];
            var v = chunk.slice(1);
            prefilters[k] = v;
            start = c;
            if (filterstring[0] in spelled) {go = 0;}
            }
        }
    //alert(opponent);
    resetFilters();
    applyPrefilters();
    makeMatchTable({}, 0);
    }

var splits_singles = ["Last 52", 'Hard', 'Clay', 'Grass', "Grand Slams", 'vs Top 10', 'vs Righties', 'vs Lefties',
                  'Best of 3', 'Best of 5']; //, "Deciding Sets"];
var splits_dubs = ["Last 52", 'Hard', 'Clay', 'Grass', "Grand Slams", 'vs Top 10', 'vs RH/RH', 'vs LH/LH',
                  'vs RH/LH', 'Deciding Sets']; //'Best of 3', 'Best of 5'];                   
                     
function makeSplitsTable(doubles=0) {
    $("#wonloss").empty();
    if (doubles == 1) {
        var splitlist = splits_dubs;
        var splits_head = matchhead_dubs;
        var splits_matches = matchmx_dubs;
        var splits_active = active_dubs;
        var splits_lastdate = lastdate_dubs;
        }
    else {
        var splitlist = splits_singles;
        var splits_head = matchhead;
        var splits_matches = matchmx;
        splits_active = active;
        splits_lastdate = lastdate;
        }
    var splits = {'Last 52': $.extend(true, {}, totals), // alternate w/ career 
                  'Hard': $.extend(true, {}, totals),
                  'Clay': $.extend(true, {}, totals),
                  'Grass': $.extend(true, {}, totals),
                  'Grand Slams': $.extend(true, {}, totals),
                  'vs Top 10': $.extend(true, {}, totals),
                  'vs Righties': $.extend(true, {}, totals),
                  'vs Lefties': $.extend(true, {}, totals),
                  'Best of 3': $.extend(true, {}, totals),
                  'Best of 5': $.extend(true, {}, totals),
                  'Deciding Sets': $.extend(true, {}, totals),
                  'vs RH/RH': $.extend(true, {}, totals),
                  'vs LH/LH': $.extend(true, {}, totals),
                  'vs RH/LH': $.extend(true, {}, totals)
                  };
    // $.extend(true, {}, totals) -- deep copy of totals
    for (m=0; m<splits_matches.length; m++) {
        var match = {}
        for (var x=0; x<splits_head.length; x++) {
            if (splits_matches[m].length <= x) {match[splits_head[x]] = '';}
            else {match[splits_head[x]] = splits_matches[m][x];}
            } 
        var msplits = [];  
        //var match = matches[m];
        match = getWinLossTiebreak(match);
        // get splits
        var keyday;
        if (splits_active == 1) {keyday = today;}
        else {keyday = splits_lastdate;}
        if (parseInt(match.date) <= keyday && parseInt(match.date) >= (keyday-10000)) {msplits.push('Last 52');}
        else {continue;} // only counting matches in last 52
        if (match.surf == "Hard") {msplits.push('Hard');}
        if (match.surf == "Clay") {msplits.push('Clay');}
        if (match.surf == "Grass") {msplits.push('Grass');}
        if (match.level == "G") {msplits.push('Grand Slams');}
        if (match.max == "3") {msplits.push('Best of 3');}
        if (match.max == "5") {msplits.push('Best of 5');}
        if (sets.length == match.max) {msplits.push('Deciding Sets');}
        if (doubles == 1) {
            if ((parseInt(match.orank) + parseInt(match.o2rank)) <= 20) {msplits.push('vs Top 10');}
            if (match.ohand == "R" && match.o2hand == "R") {msplits.push('vs RH/RH');}
            if (match.ohand == "L" && match.o2hand == "L") {msplits.push('vs LH/LH');}
            if (match.ohand == "R" && match.o2hand == "L") {msplits.push('vs RH/LH');}
            if (match.ohand == "L" && match.o2hand == "R") {msplits.push('vs RH/LH');}
            }
        else {
            if (parseInt(match.orank) <= 10) {msplits.push('vs Top 10');}
            if (match.ohand == "R") {msplits.push('vs Righties');}
            if (match.ohand == "L") {msplits.push('vs Lefties');}
            }
        for (u=0; u<stats.length; u++) {
            t = stats[u];
            for (i=0; i<msplits.length; i++) {
                //splits['All'][t] = splits['All'][t] + parseInt(match[t]);
                if (match[t] != '') {splits[msplits[i]][t] = splits[msplits[i]][t] + parseInt(match[t]);}
                }
            }
        }
    var headerrow = ['TOTALS', 'Match', 'Tiebreak', 'Ace%', '1stIn', '1st%', '2nd%', 'Hld%', 'SPW', 'Brk%', 'RPW', 'TPW', 'DR'];
    var $splittable = $('<table>').css("padding-top", "0px").css("border-top", "0px");
    var $splitchunk = $('<thead>');
    var $splitrow = $('<tr>');
    for (var i = 0; i < headerrow.length; i++) { 
        if (i == 0) {var halign = 'left';}
        else {var halign = 'right';}
        var $hspan = $('<span>').text(headerrow[i])
        if (headerrow[i] in titleTips) {$hspan.attr('title', titleTips[headerrow[i]]);}
        var $headth = $('<th>').append($hspan).css('align', halign);
        $splitrow.append($headth);
        }
    $splitchunk.append($splitrow);
    $splittable.append($splitchunk);
    var $splitchunk2  = $('<tbody>').attr('id', 'splitsbody');
    for (j=0; j<splitlist.length; j++) {
        var $splitrow = $('<tr>');
        var $splitspan = $('<span>');
        //var $splitclick = $('<span>');
        var split = splitlist[j], mt = splits[split];
        var statrow = makeSplitStatRow(mt);
        if (split == 'Last 52' || split == 'Career') {$splitspan.html(split + ' ');}
        else {$splitspan.html('&nbsp;&nbsp;' + split + ' ');}
        //$splitclick.text('(+)').attr('class', 'likelink splitclick')
        $splitrow.append($('<td>').append($splitspan)); //.append($splitclick));
        for (var i = 0; i < statrow.length; i++) {
            $splitrow.append($('<td>').text(statrow[i]).attr('align', 'right'));
            }
        if (j != 0) {$splitrow.attr('class', 'moresplits');}
        $splitchunk2.append($splitrow.attr('id', 's' + splitIds[split]))
        }
    var $yearspan = $('<span>').attr('class', 'likelink').text('show yearly totals');
    var $togglespan = $('<span>').attr('class', 'likelink').text('show splits');
    var $lastrow = $('<tr>').append($('<td>').attr('class', 'yeartoggle').append($yearspan));
    $lastrow.append($('<td>').attr('class', 'splittoggle').append($togglespan));
    for (j=2; j<headerrow.length; j++) {$lastrow.append($('<td>').text(''));}
    $splitchunk2.append($lastrow);
    $splittable.append($splitchunk2).attr('class', 'tablesorter').attr('id', 'overall');
    $splittable.attr('width', '40%').attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 0);
    $("#wonloss").html($splittable);
    
    if (typeof photog != 'undefined' && photog != '') {
        $(".moresplits").show();
        $(".splittoggle").html('<span class="likelink">hide splits</span>');
        }
    
    $(".splittoggle").click(function () {
        // close years if it's open and splits are opening
        if ($(".splittoggle").text() == 'show more splits' && $(".yeartoggle").text() == 'hide yearly totals') {
            $(".yearsplits").toggle();
            $(".yeartoggle").html('<span class="likelink">show yearly totals</span>');
            }
        $(".moresplits").toggle();
        if ($(".splittoggle").text() == 'show splits') {
            $(".splittoggle").html('<span class="likelink">hide splits</span>');
            }
        else {$(".splittoggle").html('<span class="likelink">show splits</span>');}
        })
    $(".yeartoggle").click(function () {
        // close splits if it's open and years are opening
        if ($(".yeartoggle").text() == 'show yearly totals' && $(".splittoggle").text() == 'hide splits') {
            $(".moresplits").toggle();
            $(".splittoggle").html('<span class="likelink">show splits</span>');
            }
        if ($(".yearsplits").length == 0) {addYearSplits();}
        $(".yearsplits").toggle();
        if ($(".yeartoggle").text() == 'show yearly totals') {
            $(".yeartoggle").html('<span class="likelink">hide yearly totals</span>');
            }
        else {$(".yeartoggle").html('<span class="likelink">show yearly totals</span>');}
        })
    }

function getFilterParams() {
    var params = '';
    for (j=0; j<filterlist.length; j++) {
        var filter = filterlist[j];
        if (filter in alphaFilters) {var splitter = ',';}
        else {var splitter = 'i';}
        var fselected = '.' + filter + 'selected', fdef = filter + 'def';
        var fkey = filter2url[filter]; // e.g. 'span' = 'A'
        var selnum = '';
        if ($(fselected).attr('id') != fdef) {
            $(fselected).each(function (i) {
                var selid = $(this).attr('id'), selname = selid.slice(filter.length);
                if (selid == 'spanCustomqq') {selname = 'cx'+startdate+enddate+'qq';}
                if (selid == 'rankCustomqq') {selname = 'cx'+(10000+lowrank)+(10000+highrank)+'qq';}
                if (selid == 'stats0') {selname = 'cx'+statchoice.replace(/ /g,"_")+'-'+statoperatorchoice+'-'+statinput+'qq';}
                if (selnum.length == 0) {
                    selnum = selname;
                    }
                else {
                    selnum = selnum + splitter + selname;
                    }
                });
            params = params + fkey + selnum;
            }
        }
    return params;
    }

function getPermalinkParams() {
    startwith = getFilterParams();
    var skey, sdown;
    if($("#matchheader").length == 0) {     // no results were generated 
        return startwith;
        }
    if ($('#matchheader').children(".headerSortDown").length == 0) {
        skey = $('#matchheader').children(".headerSortUp")[0].cellIndex;
        sdown = '1';
        }
    else {
        skey = $('#matchheader').children(".headerSortDown")[0].cellIndex;
        sdown = '0';
        }
    if (skey == '0' && sdown == '1') {}
    else {startwith = startwith + 's' + skey + sdown;}
    
    if (!$('#tabDubs').hasClass("tablink")) {var doubles = 1;}
    else {doubles = 0;}
    
    // a (overview) is default for doubles; o (serve stats) is default for singles
    if (!$(".statso").hasClass('likelink') && doubles == 0) {startwith += '';} 
    else if (!$(".statsa").hasClass('likelink') && doubles == 1) {startwith += '';}
    else if (!$(".statso").hasClass('likelink')) {startwith += 'o1';}
    else if (!$(".statsr").hasClass('likelink')) {startwith += 'r1';}
    else if (!$(".statsw").hasClass('likelink')) {startwith += 'w1';}
    // insert opponents, separated by commas, into url
    if ($('#h2hdef').hasClass('selected')) {}
    else {
        var oppstring = '';
        var opps = $('.h2hselected').text().split(fourspaces + fourspaces).slice(1);
        for (var i=0; i<opps.length; i++) {
            //oppstring = oppstring + opps[i].replace('\u00a0', '') + ',';
            oppstring = oppstring + opps[i].replace(/\u00a0/g, '') + ',';  //.replace(/ /g, '')
            }
        startwith = startwith + '&q=' + oppstring.slice(0,-1);
        }
    // insert excludes, separated by commas, into url
    if ($('#notdef').hasClass('selected')) {}
    else {
        var notstring = '';
        var nots = $('.notselected').text().split(fourspaces + fourspaces).slice(1);
        for (var i=0; i<nots.length; i++) {
            //notstring = notstring + nots[i].replace('\u00a0', '') + ',';
            notstring = notstring + nots[i].replace(/\u00a0/g, '') + ',';  //.replace(/ /g, '')
            }
        startwith = startwith + '&x=' + notstring.slice(0,-1);
        }
    if ($('#partnerdef').hasClass('selected')) {}
    else {
        var oppstring = '';
        var opps = $('.partnerselected').text().split(fourspaces + fourspaces).slice(1);
        for (var i=0; i<opps.length; i++) {
            //oppstring = oppstring + opps[i].replace('\u00a0', '') + ',';
            oppstring = oppstring + opps[i].replace(/\u00a0/g, '') + ',';  //.replace(/ /g, '')
            }
        startwith = startwith + '&d=' + oppstring.slice(0,-1);
        }
    if ($('#oppdef').hasClass('selected')) {}
    else {
        var oppstring = '';
        var opps = $('.oppselected').text().split(fourspaces + fourspaces).slice(1);
        for (var i=0; i<opps.length; i++) {
            //oppstring = oppstring + opps[i].replace('\u00a0', '') + ',';
            oppstring = oppstring + opps[i].replace(/\u00a0/g, '') + ',';  //.replace(/ /g, '')
            }
        startwith = startwith + '&q=' + oppstring.slice(0,-1);
        }
    if ($('#oppteamdef').hasClass('selected')) {}
    else {
        var oppstring = '';
        var opps = $('.oppteamselected').text().split(fourspaces + fourspaces).slice(1);
        for (var i=0; i<opps.length; i++) {
            //oppstring = oppstring + opps[i].replace('\u00a0', '') + ',';
            if (i % 2 == 0) {
                oppstring = oppstring + opps[i].replace(/\u00a0/g, ''); 
                }
            else {
                oppstring = oppstring + opps[i].replace(/\u00a0/g, '') + ','; 
                } 
            }
        startwith = startwith + '&t=' + oppstring.slice(0,-1);
        }
    // add &view= 
    if (!$('#tabHead').hasClass("tablink")) {startwith = startwith + "&view=h2h";}
    else if (!$('#tabEvents').hasClass("tablink")) {startwith = startwith + "&view=events";}
    else if (!$('#tabDubs').hasClass("tablink")) {startwith = startwith + "&view=doubles";}
    //else {startwith = startwith + "&view=singles";}
    return startwith;
    }

function insertNameSpaces(opponent) {
    var oppname = opponent[0];
    for (var c=1; c<opponent.length; c++) {
        if (opponent[c] == opponent[c].toUpperCase()) {
            oppname = oppname + ' ' + opponent[c];
            }
        else {oppname += opponent[c];}
        }
    oppname = oppname.replace('/ ', '/');  // for doubles
    oppname = oppname.replace(' /', '/');  // for doubles
    return oppname;
    }

function resetFilters() {
    $(".inmenu").removeClass("selected");
    $(".menudefault").addClass("selected");
    $(".header").removeClass("selected");
    var modFilterlist = filterlist.slice()
    modFilterlist.push('h2h');
    modFilterlist.push('not');
    modFilterlist.push('partner');
    modFilterlist.push('opp');
    modFilterlist.push('oppteam');
    for (i=0; i<modFilterlist.length; i++) {
        var fname = modFilterlist[i];
        var fchoice = '.' + fname + 'selected', fdef = '#' + fname + 'def';
        $(fchoice).removeClass(fchoice.slice(1));
        $(fdef).addClass(fchoice.slice(1));
        }
    }
    
function filterMatch(match, view) {
    if (view == "doubles") {
        if (genfilter('partner', 'Partner', match, match.partner) != 1) {return 0;} 
        if (genfilter('opp', 'Opponent', match, match.opp) != 1) {return 0;}
        if (genfilter('oppteam', 'Opp Team', match, match.opp) != 1) {return 0;}
        if (genfilter('hands', 'vs Hands', match, match.ohand) != 1) {return 0;}
        if (genfilter('prank', 'Partner Rank', match, match.prank) != 1) {return 0;}
        if (genfilter('phand', 'Partner Hand', match, match.phand) != 1) {return 0;}
        }
    else {
        if (genfilter('h2h', 'Head-to-Head', match, match.opp) != 1) {return 0;}     
        if (genfilter('not', 'Exclude Opp', match, match.opp) != 1) {return 0;}
        if (genfilter('crank', 'vs Curr Rk', match, match.opp) != 1) {return 0;}
        if (genfilter('hand', 'vs Hand', match, match.ohand) != 1) {return 0;}
        if (genfilter('country', 'vs Country', match, match.ocountry) != 1) {return 0;}
        if (genfilter('age', 'vs Age', match, match.obday) != 1) {return 0;}
        if (genfilter('height', 'vs Height', match, match.oht) != 1) {return 0;}
        } 
    if (genfilter('span', 'Last 52', match, match.date) != 1) {return 0;}  
    if (genfilter('rank', 'vs Rank', match, match.orank) != 1) {return 0;}
    if (genfilter('asrank', 'as Rank', match, match.rank) != 1) {return 0;}
    if (genfilter('level', 'Level', match, match.level) != 1) {return 0;}
    if (genfilter('round', 'Round', match, match.round) != 1) {return 0;}
    if (genfilter('surface', 'Surface', match, match.surf) != 1) {return 0;} 
    if (genfilter('tourney', 'Event', match, match.tourn) != 1) {return 0;} 
    if (genfilter('entry', 'Entry', match, match.entry) != 1) {return 0;} 
    if (genfilter('oentry', 'vs Entry', match, match.oentry) != 1) {return 0;} 
    if (genfilter('results', 'All', match, match.wl) != 1) {return 0;}
    if (genfilter('sets', 'All', match, match.max) != 1) {return 0;}  
    if (genfilter('scores', 'All', match, match.score) != 1) {return 0;} 
    if (genfilter('stats', 'Stats', match, match.wl) != 1) {return 0;} 
    //if (genfilter('extras', 'Extras', match, match.vidlink) != 1) {return 0;}
    return 1;
    }
    
function confirmSort(slist) {
    if ($(".headerSortDown").length > 0 || $(".headerSortUp").length > 0) {  // not the first rendering of the table; keep previous sort
        var skey, sdown;
        if ($('#matchheader').children(".headerSortDown").length == 0) {
            skey = $('#matchheader').children(".headerSortUp")[0].cellIndex;
            sdown = '1';
            }
        else {
            skey = $('#matchheader').children(".headerSortDown")[0].cellIndex;
            sdown = '0';
            }
        if (skey == '0' && sdown == '1') {}
        else {slist = [[skey, sdown]];}
        }
    else if ('sort' in prefilters) { // first rendering, use sort params from url
        var col, down;
        col = parseInt(prefilters['sort'].slice(0, -1));
        down = parseInt(prefilters['sort'].slice(-1));
        if (col == 0 && down == 1) {slist = [[0,1], [3,1]];}
        else if (col == 6 || col == 7) {slist = [[0,1], [3,1]];} // unsortable columns ... eh
        else {slist = [[col, down]];}
        }
    else {} // first rendering, no specified sort params
    return slist;
    }
    
function showDate(md) {
    var yyyy, mm, dd; // reformat date
    yyyy = md.slice(0, 4);
    mm = md.slice(4, 6);
    dd = md.slice(6);
    // non-breaking dash: &#8209;
    var showdate = parseInt(dd,10) + '\u2011' + months[mm] + '\u2011' + yyyy;
    return showdate;
    }
    
function getFilterTitle(ftitle) {
    var eightspaces = '\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0'
    var opdict = {'ge': '>=', 'le': '<=', 'gt': '>', 'lt': '<', 'eq': '='};
    for (j=0; j<filterlist.length; j++) {
        var filter = filterlist[j];
        var fselected = '.' + filter + 'selected', fdef = filter + 'def';
        if ($(fselected).attr('id') != fdef || j == 0) { // j == 0 so that time span (e.g. last 52) is always displayed)
            if (ftitle != '') {ftitle += '; ';}
            if ($(fselected).attr('id') == 'spanCustomqq') {
                var showstart = startdate.slice(6) + '-' + months[startdate.slice(4,6)] + '-' + startdate.slice(0,4);
                var showend = enddate.slice(6) + '-' + months[enddate.slice(4,6)] + '-' + enddate.slice(0,4);
                ftitle = ftitle + filteropts[filter][0] + ': ' + showstart + ' to ' + showend + ' [custom]';
                }
            else if ($(fselected).attr('id') == 'rankCustomqq') {
                ftitle = ftitle + filteropts[filter][0] + ': ' + lowrank + ' to ' + highrank + ' [custom]';
                }   
            else if ($(fselected).attr('id') == 'stats0') {
                if (statchoice in statpercs) {var statdisp = statinput+'%';}
                else if (statchoice == 'Time') {
                    var rem = (statinput % 60);
                    if (rem < 10) {var mindisp = '0'+String(rem);}
                    else {var mindisp = String(rem);}
                    var statdisp = Math.floor(statinput/60) + ':' + mindisp;
                    }
                else {var statdisp = statinput;}
                ftitle = ftitle + filteropts[filter][0] + ': ' + statchoice + ' ' + opdict[statoperatorchoice] + ' ' + statdisp + ' [custom]';
                //add percent sign as necessary?
                }             
            else {ftitle = ftitle + filteropts[filter][0] + ': ' + $.trim($(fselected).text());}
            ftitle = ftitle.replace(/\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0\u00a0/g, ', ')
            // add comma if more than one fselected?
            }
        }
    return ftitle;
    }
  
function renderResults(stdscores, showstats, hdrs, defSort) {
    // defSort: 1 = return to defaults; 0 = check if already sorted and keep that
    var currentfilter = $.extend(true, {}, totals);
    // decide now on table sorting
    var slist = [[0,1], [3,1]];
    if (defSort == 0) {slist = confirmSort(slist);}

    var $matchtable = $('<table>');
    var $mthead = $('<thead>');
    var $mtheaderrow = $('<tr>').attr('id', 'matchheader'); 
    var $mtfoot = $('<tfoot>');
    var $mtfooterrow = $('<tr>').attr('id', 'matchfooter').attr('background-color', '#FFF'); // color weirdness?  
    var $mtbody = $('<tbody>');
    var rightaligns = {4: '', 5: '', 9: '', 10: '', 11: '', 12: '', 13: '', 14: '', 15: '', 16: ''};
    for (var s = 0; s < matchmx.length; s++) {   
        if (s == 0) {
            // header row
            var cells = [];
            for (var i = 0; i < hdrs.length; i++) {
                //var $mthcell = $('<th>');
                var $mthspan = $('<span>').text(hdrs[i]);
                if (hdrs[i] in titleTips) {$mthspan.attr('title', titleTips[hdrs[i]]);}
                var $mthcell = $('<th>').append($mthspan);
                if (i in rightaligns) {$mthcell.attr('align', 'right');}
                $mtheaderrow.append($mthcell);
                }
            $mthead.append($mtheaderrow);
            $matchtable.append($mthead);
            // footer row
            var fcells = [];
            for (var i = 0; i < hdrs.length; i++) {
                //var $mftcell = $('<tr>'); // check
                if (i == 1 && !$("#spanCareerqq").hasClass('selected')) { // && career is not selected
                    var $mftspan = $('<span>').text('Show Career').attr('font-style', 'italic');
                    $mftspan.attr('id', 'careerclick').attr('class', 'likelink');
                    }
                else {
                    var $mftspan = $('<span>').text('');
                    }
                var $mftcell = $('<td>').append($mftspan);
                $mtfooterrow.append($mftcell);
                }
            $mtfoot.append($mtfooterrow);
            $matchtable.append($mtfoot);
            }            
            
        // when adding a new column, esp. if just in some players' js files, add the header to 'matchhead' above
        // this fills in the missing cells
        var match = {}
        for (var x=0; x<matchhead.length; x++) {
            if (matchmx[s].length <= x) {match[matchhead[x]] = '';}
            else {match[matchhead[x]] = matchmx[s][x];}
            } 
            
        if (filterMatch(match, 'results') != 1) {continue;}

        var $mtrow = $('<tr>');        
        var showdate = showDate(match.date);
        var yyyy, mm, dd; // reformat date
        yyyy = match.date.slice(0, 4);
        mm = match.date.slice(4, 6);
        dd = match.date.slice(6);
        //if (typeof dob === 'undefined') {}
        //else {
        //    var dobStr = dob+'';
        //    var dobObj  = new Date(dobStr.slice(0,4), dobStr.slice(4,6), dobStr.slice(6));
        //    var tdObj  = new Date(yyyy, mm, dd);
        //    var days = parseInt((tdObj.getTime() - dobObj.getTime())/one_day);
        //    var years = parseInt(days/365);
        //    var plusdays = days % 365;
        //    if (plusdays < 10) {plusdays = '00'+plusdays;}
        //    else if (plusdays < 100) {plusdays = '0'+plusdays;}
        //    $datecell.attr('title', 'Age: '+years+'.'+plusdays);
        //    }
        if (yyyy >= 2085 && yyyy < 1916) {
            var wdate = yyyy+'-'+mm+'-'+dd;
            var wlink = 'https://tennisabstract.herokuapp.com/ATP/Rankings/'+wdate+'/';
            //$datecell.attr('href', wlink);  
            //var $datecell = $('<td>').append($('<a>').text(showdate).attr('href', wlink));
            var $datecell = $('<td>').text(showdate);
            }          
        else {var $datecell = $('<td>').text(showdate).attr('class', 'tdate');}
        $mtrow.append($datecell);
        
        var showtourn = match.tourn;
        if (match.tourn.indexOf('Davis C') != -1) {
            $mtrow.append($('<td>').text(match.tourn));
            }
        else if (match.level == 'S' | match.level == 'E') {
            $mtrow.append($('<td>').text(match.tourn));
            }
        else if (match.tourn.slice(0,3) == 'ATP' && (yyyy == '2022' | yyyy == '2024' || yyyy == '2023')) {
            $mtrow.append($('<td>').text(match.tourn.slice(4)));
            }
        else if (match.tourn.slice(-10) == 'Challenger' && (yyyy == '2022' | yyyy == '2024' || yyyy == '2023')) {
            $mtrow.append($('<td>').text(match.tourn.slice(0,-11)+' CH'));
            }
        else {
            if (match.tourn.slice(-2) == ' Q') {
                var showtourn = match.tourn.slice(0, -2);
                var qtag = '&f=B8';
                }
            else {
                var showtourn = match.tourn;
                var qtag = '';}
            var tlink = 'https://www.tennisabstract.com/cgi-bin/tourney.cgi?t=' + yyyy + showtourn.replace(/ /g, '_').replace(/'/g, 'xx') + qtag;
            $mtrow.append($('<td>').append($('<a>').text(match.tourn).attr('href', tlink)));
            }
        $mtrow.append($('<td>').text(match.surf));
        $mtrow.append($('<td>').text(match.round));
        $mtrow.append($('<td>').text(match.rank).attr('align', 'right'));
        $mtrow.append($('<td>').text(match.orank).attr('align', 'right'));

        var $pentry = $('<span>'), $pname = $('<span>'), $oppentry = $('<span>'), $opplink = $('<span>'), $oppcc = $('<span>')  // player entry
        if (match.seed != '') {$pentry.text('(' + match.seed + ')')}
        else if (match.entry != '') {$pentry.text('(' + match.entry + ')')}
        else {$pentry.text('');}
        $pname.text(lastname).css('font-weight', 'bold'); // player name
        if (match.oseed != '') {$oppentry.text('(' + match.oseed + ')')} // opp entry
        else if (match.oentry != '') {$oppentry.text('(' + match.oentry + ')')}
        else {$oppentry.text('');}
        var nlink = 'https://www.tennisabstract.com/cgi-bin/player.cgi?p=' + match.opp.replace(/ /g, '') // opp link
        var $olk = $('<a>').text(match.opp).attr('href', nlink);
        $opplink.append($olk);
        $oppcc.text(' [' + match.ocountry + ']'); 
        var $onespace = $('<span>').text('\u00a0')
        var $onespace2 = $('<span>').text('\u00a0')
        if (match.score == '' && (yyyy == '2023' | yyyy == '2024')) {var $deflink = $('<span>').text('vs');}
        else {var $deflink = $('<span>').text('d.');}
        if ($('#h2hdef').hasClass('selected')) {$deflink.attr('class', 'likelink h2hclick');}
        var $matchcell = $('<td>').attr('class', match.opp.replace(/ /g, ''));
        if (match.wl == "W" || match.wl == 'U') {
            $matchcell.append($pentry).append($pname).append($onespace).append($deflink).append($onespace2).append($oppentry).append($opplink).append($oppcc);
            }
        else {
            $matchcell.append($oppentry).append($opplink).append($oppcc).append($onespace).append($deflink).append($onespace2).append($pentry).append($pname);
            }
        $mtrow.append($matchcell);
        if (match.score == '' && (yyyy == '2023' | yyyy == '2024')) {
            var $scorecell = $('<a>').text('Live Scores').attr('href', 'https://www.livescore.in/tennis/').attr('target', '_blank');
            }
        else if (stdscores == true || match.score == 'W/O') {
            if (match.whserver != '' && match.whserver == 'magic') { // re-activate when match.py working
                if (match.level == 'S') {var mlev = 'FUT';}
                else if (match.level == 'C') {mlev = 'CHA';}
                else if (match.level == 'Q') {mlev = 'AQU';}
                else if (match.round == 'Q1' || match.round == 'Q2' || match.round == 'Q3') {mlev = 'AQU';}
                else {mlev = 'ATP';}
                var wlink = 'https://tennisabstract.com/cgi-bin/match.py?q=' + match.matchid + '/' + mlev + '/';
                wlink = wlink + showtourn.replace(/ /g, '_') + '/' + match.round + '/';
                if (match.wl == 'W') {
                    wlink = wlink + fullname.replace(/ /g, '_') + '/vs/' + match.opp.replace(/ /g, '_') + '/1'
                    }
                else {
                    wlink = wlink + match.opp.replace(/ /g, '_') + '/vs/' + fullname.replace(/ /g, '_') + '/0'
                    }
                var $scorecell = $('<a>').text(match.score).attr('href', wlink).attr('target', '_blank').attr('title', "Point-by-point stats for this match (new window)");                
                }
            else {
                var $scorecell = $('<span>').text(match.score);
                }
            }
        else if (match.wl == 'L') {
            var sets = match.score.split(' '), newscore = '';
            for (k=0; k<sets.length; k++) {
                var st = sets[k];
                if (st == 'RET') {newscore = newscore + 'RET ';}
                else if (st[2] == '-') {newscore = newscore + st.slice(3) + '-' + st.slice(0, 2) + ' ';}
                else {newscore = newscore + st[2] + st[1] + st[0] + st.slice(3) + ' ';}
                // what about 70-68, 11-9, etc?
                }
            newscore = newscore.slice(0, -1);
            //$mtrow.append($('<td>').text(newscore));
            var $scorecell = $('<span>').text(newscore);
            }
        else {
            //$mtrow.append($('<td>').text(match.score));
            var $scorecell = $('<span>').text(match.score);
            }
        var $scorelinks = $('<td>').append($scorecell);
        $mtrow.append($scorelinks);
        var $matchlinks = $('<td>');
        if (match.chartlink != "") {
            var clink = 'https://www.tennisabstract.com/charting/' + match.chartlink;
            var $chartlink = $('<a>').text("(ch)").attr('href', clink).attr('target', '_blank').attr('title', "Charting-Based Stats (new window)");
            var $onespace3 = $('<span>').text('\u00a0');
            $matchlinks.append($onespace3).append($chartlink);
            }
        if (match.pslink != "") {
            var $onespace4 = $('<span>').text('\u00a0');
            var plink = 'https://www.tennisabstract.com/cgi-bin/slamWinProbability.py?m=' + match.pslink;
            var $pslink = $('<a>').text("(wp)").attr('href', plink).attr('target', '_blank').attr('title', "Point-by-Point Data (new window)");
            $matchlinks.append($onespace4).append($pslink);
            }
        //if (match.vidlink != "") {
        //   var $onespace5 = $('<span>').text('\u00a0');
        //    var $vlink = $('<a>').text("(vid)").attr('href', match.vidlink).attr('target', '_blank').attr('title', "Full Match Video (new window)");
        //    $matchlinks.append($onespace5).append($vlink);
        //    }
        $mtrow.append($matchlinks);
        if (match.score == 'W/O' || match.pts == '') {  // matches with no stats, leave blank
            var statrow = ['', '', '', '', '', '', ''], showtime = '', domratio = '';
            }
        else {
            var rpw = 1 - (parseInt(match.ofwon) + parseInt(match.oswon))/match.opts; // dominance ratio:
            var spl = 1 - ((parseInt(match.fwon) + parseInt(match.swon))/match.pts);
            var num = rpw/spl, dec = 2;
            var domratio = alignRound(rpw/spl, dec);
            var matchtime = match.time;
            if (matchtime == '') {var showtime = '';}
            else {
                var hours = parseInt(matchtime/60);
                var minutes = matchtime % 60;
                if (minutes < 10) {minutes = '0' + minutes;}
                var showtime = hours + ':' + minutes;
                }
            var hours = parseInt(matchtime/60);
            var minutes = matchtime % 60;
            if (minutes < 10) {minutes = '0' + minutes;}
            if (showstats == 'o') {
                var acerate = alignRound((match.aces/match.pts), 1, 1);
                var dfrate = alignRound((match.dfs/match.pts), 1, 1);
                var firstin = alignRound((match.firsts/match.pts), 1, 1);
                var fwin = alignRound((match.fwon/match.firsts), 1, 1);
                var swin = alignRound((match.swon/(match.pts-match.firsts)), 1, 1);
                //var bksaved = alignRound((match.saved/match.chances), 1, 1) + ' (' + match.saved + '/' + match.chances + ')';
                var bksaved = match.saved + '/' + match.chances;
                var statrow = [domratio, acerate, dfrate, firstin, fwin, swin, bksaved];
                }
            else if (showstats == 'r') {
                var pointswon = parseInt(match.fwon) + parseInt(match.swon) + (match.opts - match.ofwon - match.oswon);
                var tpw = alignRound((pointswon/(parseInt(match.pts) + parseInt(match.opts))), 1, 1);
                var rpw = alignRound(1 - ((parseInt(match.ofwon) + parseInt(match.oswon))/match.opts), 1, 1);
                var vace = alignRound((match.oaces/match.opts), 1, 1);
                var r1pw = alignRound(1 - (match.ofwon/match.ofirsts), 1, 1);
                var r2pw = alignRound(1 - (match.oswon/(match.opts-match.ofirsts)), 1, 1);
                //var bkconv = alignRound(1 - (match.osaved/match.ochances), 1, 1) + ' (' + (match.ochances - match.osaved) + '/' + match.ochances + ')';
                var bkconv = (match.ochances - match.osaved) + '/' + match.ochances;
                var statrow = [domratio, tpw, rpw, vace, r1pw, r2pw, bkconv];
                }
            else if (showstats == 'w') {
                var pointswon = parseInt(match.fwon) + parseInt(match.swon) + (match.opts - match.ofwon - match.oswon);
                var tpw = alignRound((pointswon/(parseInt(match.pts) + parseInt(match.opts))), 1, 1);
                var rpw = alignRound(1 - ((parseInt(match.ofwon) + parseInt(match.oswon))/match.opts), 1, 1);
                var vace = alignRound((match.oaces/match.opts), 1, 1);
                var r1pw = alignRound(1 - (match.ofwon/match.ofirsts), 1, 1);
                var r2pw = alignRound(1 - (match.oswon/(match.opts-match.ofirsts)), 1, 1);
                var bkconv = alignRound(1 - (match.osaved/match.ochances), 1, 1) + ' (' + (match.ochances - match.osaved) + '/' + match.ochances + ')';
                var tp = parseInt(match.pts) + parseInt(match.opts)
                var statrow = [tp, match.aces, match.dfs, match.pts, match.firsts, match.pts-match.firsts, match.oaces];
                }                
            }
        //$mtrow.append($('<td>').text(domratio).attr('align', 'right'));;
        for (var i = 0; i < statrow.length; i++) {
            $mtrow.append($('<td>').text(statrow[i]).attr('align', 'right'));
            }
        $mtrow.append($('<td>').text(showtime).attr('align', 'right'));
        $mtbody.append($mtrow);
        var matchnew = getWinLossTiebreak(match);
        for (u=0; u<stats.length; u++) {
            t = stats[u];
            if (matchnew[t] != '') {currentfilter[t] = currentfilter[t] + parseInt(matchnew[t]);}
            }
        }

    // back to match table
    if ($mtbody.children().length == 0) {
        if (!$("#spanCareerqq").hasClass('selected')) { // career is not selected
            //var $mftspan = $('<span>').text('show career').attr('font-style', 'italic');
            //$mftspan.attr('id', 'careerclick').attr('class', 'likelink');
            var mft = '<span id="careerclick" class="likelink">Show career</span>, c';
            }
        else {var mft = 'C';}
        pTable = '<p id="matches">&nbsp;<br/>&nbsp;<br/>Your filters returned no matches. ' + mft + 'hange a filter or two, or press the '
        pTable = pTable + '<span class="menureset2 likelink">reset button</span>.</p>'
        $("#matches").replaceWith(pTable);
        }
    else {
        $matchtable.append($mtbody).attr('id', 'matches').attr('class', 'tablesorter');
        $matchtable.attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
        $("#stats").html($matchtable);
        $("#matches").tablesorter( {sortList: slist,
                                    headers: {0: {sorter:'dates'},
                                    	      1: {sorter:false},	
                                              3: {sorter:'rounds'},
                                              4: {sorter:'ranks'},
                                              5: {sorter:'ranks'},
                                              6: {sorter:false}, // matchup
                                              7: {sorter:false}, // score -- more I can do with this?
                                              8: {sorter:false},
                                              9: {sorter:'descNum'},
                                              10: {sorter:'descNum'},
                                              11: {sorter:'ascNum'},
                                              12: {sorter:'descNum'},
                                              13: {sorter:'descNum'},
                                              14: {sorter:'descNum'},
                                              15: {sorter:false},
                                              16: {sorter:'descNum'}
                                              }
                                   });
        }
    var currentParams = getFilterParams();
    var opplist = [], notlist = [];
    if ($('#h2hdef').hasClass('selected') && $('#notdef').hasClass('selected')) {var opplist = [];} // no h2hs or NOTs
    else if (!$('#notdef').hasClass('selected')) {var notlist = $('.notselected').text().split(fourspaces + fourspaces).slice(1);}
    else {var opplist = $('.h2hselected').text().split(fourspaces + fourspaces).slice(1);}
    var statrow = makeSplitStatRow(currentfilter);
    if ($mtbody.children().length == 0) {}
    else if (currentParams == '' && (opplist.length == 0 && notlist.length == 0)) {}
    else {
        if (opplist.length == 0 && notlist.length == 0) {var rowid = '#f' + currentParams;}
        else if (opplist.length == 0) {
            var nots = $('.notselected').text().replace(/\u00a0/g, '');
            var rowid = '#f' + currentParams + '-' + nots;
            }
        else {
            var opps = $('.h2hselected').text().replace(/\u00a0/g, '');
            var rowid = '#f' + currentParams + '-' + opps;
            }
        if ($(rowid).length > 0) {$(rowid).remove();} // if same filter is already present, delete
        else if ($('.filtersplitrow').length == 5) {$('.filtersplitrow').last().remove();} // if 5 already, delete last

        var $filtrow = $('<tr>').attr('class', 'filtersplitrow')
        $filtrow.attr('id', rowid.slice(1));
        var ftitle = '';
        if (opplist.length == 0) {} // get label
        else {
            for (k=0; k<opplist.length; k++) {
                //var oppname = insertNameSpaces(opplist[k]);
                var ftitle = ftitle + 'Opponent: ' + opplist[k] + '; '; // need to parse this for initials? last name?
                }
            ftitle = ftitle.slice(0, -2); // delete final semi-colon and space
            }
        if (notlist.length == 0) {} // get label
        else {
            for (k=0; k<notlist.length; k++) {
                //var oppname = insertNameSpaces(opplist[k]);
                var ftitle = ftitle + 'Exclude: ' + notlist[k] + '; '; // need to parse this for initials? last name?
                }
            ftitle = ftitle.slice(0, -2); // delete final semi-colon and space
            }  
        ftitle = getFilterTitle(ftitle);      

        var $nameital = ($('<span>').css('font-style', 'italic').attr('class', 'filterlabel'));
        if (ftitle.length > 25) { // too long, replace end with ..., add title, etc.
            $nameital.attr('title', ftitle.replace(/; /g, '\n')).css('cursor', 'pointer').text(' ' + ftitle.slice(0,22) + '... ');
            }
        else {$nameital.text(' ' + ftitle + ' ');}
        
        var $closebutton = ($('<span>').text("[x]").attr('class', 'rowcloser likelink')) // add button to close
        //var $splitclick = $('<span>').text('(+)').attr('class', 'likelink splitclick');
        
        $filtrow.append($('<td>').append($closebutton).append($nameital)); //.append($splitclick));
        for (var i = 0; i < statrow.length; i++) {
            var $fcell = $('<td>').attr('align', 'right').text(statrow[i]);
            $filtrow.append($fcell);
            }
        $("#splitsbody").prepend($filtrow)

        $(".rowcloser").click(function () {// attach handler to .rowcloser
            $(this).parent().parent().remove();
            })
        }
        
    // add title above matches
    if (ftitle == "" || ftitle == undefined) {var ftitle = 'Last 52';}
    else {var tabtitle = 'Matches: '+ftitle;} //.replace(/; /g, ' > ');}
    var wlrec = '(' + statrow[0].slice(0,statrow[0].indexOf('(')-1) + ')';
    $("#tablelabel").html('<b>Matches '+wlrec+' > '+ftitle+'</b>');
        
    // how to count filters applied?
    if (currentParams == '' && (opplist.length == 0 && notlist.length == 0)) {}
    else if ($mtbody.children().length == 0) {} // no matches -- keep this or not?
    else {
        var $bmatches = $('<span>').text('Apply filters to another player: ');
        $bmatches.append($('<input>').attr('id', 'paramsearch'));
        $('#matches').after($bmatches);
        }
    $(".h2hclick").click(function () {
        opponent = $(this).parent().attr('class');
        var withspaces = insertNameSpaces(opponent);
        setPlayerFilter(withspaces, 1, 0);
        if ($("#h2hhead").hasClass("closed")) {
            $("#h2hhead").removeClass("closed").addClass("open") //.addClass("selected");
            $(".h2hchoice").toggle();
            }
        })
    $(".menureset2").click(function () {
        resetFilters();
        // close open menus?
        makeMatchTable({}, 1);
        });
    $("#careerclick").click(function () {
        if ($("#spanhead").hasClass("closed")) {
            $("#spanhead").removeClass("closed").addClass("open") //.addClass("selected");
            $(".spanchoice").toggle();
            }
        $(".spanchoice").removeClass("selected").removeClass('spanselected');
        $("#spanCareerqq").addClass('selected').addClass('spanselected');
        makeMatchTable({}, 0);
        })
    }
    
function renderDoubles(stdscores, showstats, hdrs, defSort) {
    // defSort: 1 = return to defaults; 0 = check if already sorted and keep that
    var currentfilter = $.extend(true, {}, totals);
    // decide now on table sorting
    var slist = [[0,1], [3,1]];
    if (defSort == 0) {slist = confirmSort(slist);}

    var $matchtable = $('<table>');
    var $mthead = $('<thead>');
    var $mtheaderrow = $('<tr>').attr('id', 'matchheader'); 
    var $mtfoot = $('<tfoot>');
    var $mtfooterrow = $('<tr>').attr('id', 'matchfooter').attr('background-color', '#FFF'); // color weirdness?  
    var $mtbody = $('<tbody>');
    if (showstats == "a") {
        var rightaligns = {4: '', 5: '', 8: '', 9: '', 10: '', 11: '', 12: '', 13: '', 14: '', 15: ''};
        }
    else {
        var rightaligns = {6: '', 7: '', 8: '', 9: '', 10: '', 11: '', 12: '', 13: '', 14: '', 15: ''};
        }
    for (var s = 0; s < matchmx_dubs.length; s++) {   
        if (s == 0) {
            // header row
            var cells = [];
            for (var i = 0; i < hdrs.length; i++) {
                //var $mthcell = $('<th>');
                var $mthspan = $('<span>').text(hdrs[i]);
                if (hdrs[i] in titleTips) {$mthspan.attr('title', titleTips[hdrs[i]]);}
                var $mthcell = $('<th>').append($mthspan);
                if (i in rightaligns) {$mthcell.attr('align', 'right');}
                $mtheaderrow.append($mthcell);
                }
            $mthead.append($mtheaderrow);
            $matchtable.append($mthead);
            // footer row
            var fcells = [];
            for (var i = 0; i < hdrs.length; i++) {
                //var $mftcell = $('<tr>'); // check
                if (i == 1 && !$("#spanCareerqq").hasClass('selected')) { // && career is not selected
                    var $mftspan = $('<span>').text('Show Career').attr('font-style', 'italic');
                    $mftspan.attr('id', 'careerclick').attr('class', 'likelink');
                    }
                else {
                    var $mftspan = $('<span>').text('');
                    }
                var $mftcell = $('<td>').append($mftspan);
                $mtfooterrow.append($mftcell);
                }
            $mtfoot.append($mtfooterrow);
            $matchtable.append($mtfoot);
            }            
            
        // when adding a new column, esp. if just in some players' js files, add the header to 'matchhead' above
        // this fills in the missing cells
        var match = {}
        for (var x=0; x<matchhead_dubs.length; x++) {
            if (matchmx_dubs[s].length <= x) {match[matchhead_dubs[x]] = '';}
            else {match[matchhead_dubs[x]] = matchmx_dubs[s][x];}
            } 
            
        if (filterMatch(match, 'doubles') != 1) {continue;}

        var $mtrow = $('<tr>');        
        var showdate = showDate(match.date);
        var yyyy, mm, dd; // reformat date
        yyyy = match.date.slice(0, 4);
        mm = match.date.slice(4, 6);
        dd = match.date.slice(6);
        //if (typeof dob === 'undefined') {}
        //else {
        //    var dobStr = dob+'';
        //    var dobObj  = new Date(dobStr.slice(0,4), dobStr.slice(4,6), dobStr.slice(6));
        //    var tdObj  = new Date(yyyy, mm, dd);
        //    var days = parseInt((tdObj.getTime() - dobObj.getTime())/one_day);
        //    var years = parseInt(days/365);
        //    var plusdays = days % 365;
        //    if (plusdays < 10) {plusdays = '00'+plusdays;}
        //    else if (plusdays < 100) {plusdays = '0'+plusdays;}
        //    $datecell.attr('title', 'Age: '+years+'.'+plusdays);
        //    }
        if (yyyy >= 2085 && yyyy < 1916) {
            var wdate = yyyy+'-'+mm+'-'+dd;
            var wlink = 'https://tennisabstract.herokuapp.com/ATP/Rankings/'+wdate+'/';
            //$datecell.attr('href', wlink);  
            var $datecell = $('<td>').append($('<a>').text(showdate).attr('href', wlink));
            }          
        else {var $datecell = $('<td>').text(showdate).attr('class', 'tdate');}
        $mtrow.append($datecell);
        
        var showtourn = match.tourn;
        if (match.tourn.indexOf('Davis C') != -1) {
            $mtrow.append($('<td>').text(match.tourn));
            }
        else if (match.level == 'S' | match.level == 'E') {
            $mtrow.append($('<td>').text(match.tourn));
            }
        else if (match.tourn.slice(0,3) == 'ATP' && (yyyy == '2022' | yyyy == '2024' || yyyy == '2023')) {
            $mtrow.append($('<td>').text(match.tourn.slice(4)));
            }
        else if (match.tourn.slice(-10) == 'Challenger' && (yyyy == '2022' | yyyy == '2024' || yyyy == '2023')) {
            $mtrow.append($('<td>').text(match.tourn.slice(0,-11)+' CH'));
            }
        else {
            if (match.tourn.slice(-2) == ' Q') {
                var showtourn = match.tourn.slice(0, -2);
                var qtag = '&f=B8';
                }
            else {
                var showtourn = match.tourn;
                var qtag = '';}
            var tlink = 'https://www.tennisabstract.com/cgi-bin/tourney.cgi?t=' + yyyy + showtourn.replace(/ /g, '_').replace(/'/g, 'xx') + qtag;
            $mtrow.append($('<td>').append($('<a>').text(match.tourn).attr('href', tlink)));
            }
        $mtrow.append($('<td>').text(match.surf));
        $mtrow.append($('<td>').text(match.round));
        
        if (showstats == "a") {
            $mtrow.append($('<td>').text(match.rank + '/' + match.prank).attr('align', 'right'));
            $mtrow.append($('<td>').text(match.orank + '/' + match.o2rank).attr('align', 'right'));
            var partner_name = match.partner;
            var opp_name = match.opp;
            var opp2_name = match.opp2;
            }
        else {
            partner_name = match.partnerlast;
            opp_name = match.olast;
            opp2_name = match.o2last;
            }

        var $pentry = $('<span>'), $pname = $('<span>'), $oppentry = $('<span>'), $opplink = $('<span>'), $oppcc = $('<span>')  // player entry 
        var $partnerlink = $('<span>'), $opp2link = $('<span>'), $partnercc = $('<span>')
        if (match.seed != '') {$pentry.text('(' + match.seed + ')')}
        else if (match.entry != '') {$pentry.text('(' + match.entry + ')')}
        else {$pentry.text('');}
        
        $pname.text(lastname).css('font-weight', 'bold'); // player name

        var partnerlink = 'https://www.tennisabstract.com/cgi-bin/player.cgi?p=' + match.partner.replace(/ /g, '') 
        var $partnerlk = $('<a>').text(partner_name).attr('href', partnerlink);
        $partnerlink.append($partnerlk);
        $partnercc.text(' [' + match.pcountry + ']');
                
        if (match.oseed != '') {$oppentry.text('(' + match.oseed + ')')} // opp entry
        else if (match.oentry != '') {$oppentry.text('(' + match.oentry + ')')}
        else {$oppentry.text('');}
        
        var nlink = 'https://www.tennisabstract.com/cgi-bin/player.cgi?p=' + match.opp.replace(/ /g, '') // opp link
        var $olk = $('<a>').text(opp_name).attr('href', nlink);
        $opplink.append($olk);
        
        var n2link = 'https://www.tennisabstract.com/cgi-bin/player.cgi?p=' + match.opp2.replace(/ /g, '') 
        var $o2lk = $('<a>').text(opp2_name).attr('href', n2link);
        $opp2link.append($o2lk);
        
        if (match.ocountry == match.o2country) {
            $oppcc.text(' [' + match.ocountry + ']');
            }
        else {
            $oppcc.text(' [' + match.ocountry + '/' + match.o2country + ']'); 
            }
        var $onespace = $('<span>').text('\u00a0')
        var $onespace2 = $('<span>').text('\u00a0')
        var $slash = $('<span>').text('/')
        var $slash2 = $('<span>').text('/')
        if (match.score == '' && (yyyy == '2023' | yyyy == '2024')) {var $deflink = $('<span>').text('vs');}
        else {var $deflink = $('<span>').text('d.');}
        //if ($('#h2hdef').hasClass('selected')) {$deflink.attr('class', 'likelink h2hclick');}
        var $matchcell = $('<td>')  //.attr('class', match.opp.replace(/ /g, ''));
        if (match.wl == "W" || match.wl == 'U') {
            $matchcell.append($pentry).append($pname).append($slash).append($partnerlink).append($partnercc).append($onespace).append($deflink).append($onespace2).append($oppentry).append($opplink).append($slash2).append($opp2link).append($oppcc);
            }
        else {
            $matchcell.append($oppentry).append($opplink).append($slash2).append($opp2link).append($oppcc).append($onespace).append($deflink).append($onespace2).append($pentry).append($pname).append($slash).append($partnerlink).append($partnercc);
            }
        $mtrow.append($matchcell);
        if (match.score == '' && (yyyy == '2023' | yyyy == '2024')) {
            var $scorecell = $('<a>').text('Live Scores').attr('href', 'https://www.livescore.in/tennis/').attr('target', '_blank');
            }
        else {
            //$mtrow.append($('<td>').text(match.score));
            var $scorecell = $('<span>').text(match.score);
            }
        var $scorelinks = $('<td>').append($scorecell);
        $mtrow.append($scorelinks);

        if (match.score == 'W/O' || match.pts == '') {  // matches with no stats, leave blank
            var statrow = ['', '', '', '', '', '', ''], showtime = '', domratio = '';
            if (showstats == "a") {statrow = [''];}
            }
        else {
            var rpw = 1 - (parseInt(match.ofwon) + parseInt(match.oswon))/match.opts; // dominance ratio:
            var spl = 1 - ((parseInt(match.fwon) + parseInt(match.swon))/match.pts);
            var num = rpw/spl, dec = 2;
            var domratio = alignRound(rpw/spl, dec);
            var matchtime = match.time;
            if (matchtime == '') {var showtime = '';}
            else {
                var hours = parseInt(matchtime/60);
                var minutes = matchtime % 60;
                if (minutes < 10) {minutes = '0' + minutes;}
                var showtime = hours + ':' + minutes;
                }
            var hours = parseInt(matchtime/60);
            var minutes = matchtime % 60;
            if (minutes < 10) {minutes = '0' + minutes;}
            if (showstats == 'a') {
                var statrow = [domratio];
                }
            else if (showstats == 'o') {
                var acerate = alignRound((match.aces/match.pts), 1, 1);
                var dfrate = alignRound((match.dfs/match.pts), 1, 1);
                var firstin = alignRound((match.firsts/match.pts), 1, 1);
                var fwin = alignRound((match.fwon/match.firsts), 1, 1);
                var swin = alignRound((match.swon/(match.pts-match.firsts)), 1, 1);
                //var bksaved = alignRound((match.saved/match.chances), 1, 1) + ' (' + match.saved + '/' + match.chances + ')';
                var bksaved = match.saved + '/' + match.chances;
                var statrow = [domratio, acerate, dfrate, firstin, fwin, swin, bksaved];
                }
            else if (showstats == 'r') {
                var pointswon = parseInt(match.fwon) + parseInt(match.swon) + (match.opts - match.ofwon - match.oswon);
                var tpw = alignRound((pointswon/(parseInt(match.pts) + parseInt(match.opts))), 1, 1);
                var rpw = alignRound(1 - ((parseInt(match.ofwon) + parseInt(match.oswon))/match.opts), 1, 1);
                var vace = alignRound((match.oaces/match.opts), 1, 1);
                var r1pw = alignRound(1 - (match.ofwon/match.ofirsts), 1, 1);
                var r2pw = alignRound(1 - (match.oswon/(match.opts-match.ofirsts)), 1, 1);
                //var bkconv = alignRound(1 - (match.osaved/match.ochances), 1, 1) + ' (' + (match.ochances - match.osaved) + '/' + match.ochances + ')';
                var bkconv = (match.ochances - match.osaved) + '/' + match.ochances;
                var statrow = [domratio, tpw, rpw, vace, r1pw, r2pw, bkconv];
                }
            else if (showstats == 'w') {
                var pointswon = parseInt(match.fwon) + parseInt(match.swon) + (match.opts - match.ofwon - match.oswon);
                var tpw = alignRound((pointswon/(parseInt(match.pts) + parseInt(match.opts))), 1, 1);
                var rpw = alignRound(1 - ((parseInt(match.ofwon) + parseInt(match.oswon))/match.opts), 1, 1);
                var vace = alignRound((match.oaces/match.opts), 1, 1);
                var r1pw = alignRound(1 - (match.ofwon/match.ofirsts), 1, 1);
                var r2pw = alignRound(1 - (match.oswon/(match.opts-match.ofirsts)), 1, 1);
                var bkconv = alignRound(1 - (match.osaved/match.ochances), 1, 1) + ' (' + (match.ochances - match.osaved) + '/' + match.ochances + ')';
                var tp = parseInt(match.pts) + parseInt(match.opts)
                var statrow = [tp, match.aces, match.dfs, match.pts, match.firsts, match.pts-match.firsts, match.oaces];
                }                
            }
        //$mtrow.append($('<td>').text(domratio).attr('align', 'right'));;
        for (var i = 0; i < statrow.length; i++) {
            $mtrow.append($('<td>').text(statrow[i]).attr('align', 'right'));
            }
        $mtrow.append($('<td>').text(showtime).attr('align', 'right'));
        $mtbody.append($mtrow);
        var matchnew = getWinLossTiebreak(match);
        for (u=0; u<stats.length; u++) {
            t = stats[u];
            if (matchnew[t] != '') {currentfilter[t] = currentfilter[t] + parseInt(matchnew[t]);}
            }
        }

    // back to match table
    if ($mtbody.children().length == 0) {
        if (!$("#spanCareerqq").hasClass('selected')) { // career is not selected
            //var $mftspan = $('<span>').text('show career').attr('font-style', 'italic');
            //$mftspan.attr('id', 'careerclick').attr('class', 'likelink');
            var mft = '<span id="careerclick" class="likelink">Show career</span>, c';
            }
        else {var mft = 'C';}
        pTable = '<p id="matches">&nbsp;<br/>&nbsp;<br/>Your filters returned no matches. ' + mft + 'hange a filter or two, or press the '
        pTable = pTable + '<span class="menureset2 likelink">reset button</span>.</p>'
        $("#matches").replaceWith(pTable);
        }
    else {
        $matchtable.append($mtbody).attr('id', 'matches').attr('class', 'tablesorter');
        $matchtable.attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
        $("#stats").html($matchtable);
        if (showstats == "a") {
            $("#matches").tablesorter( {sortList: slist,
                                    headers: {0: {sorter:'dates'},
                                    	      1: {sorter:false},	
                                              3: {sorter:'rounds'},
                                              4: {sorter:'ranks'},
                                              5: {sorter:'ranks'},
                                              6: {sorter:false}, // matchup
                                              7: {sorter:false}, // score -- more I can do with this?
                                              8: {sorter:'descNum'},
                                              9: {sorter:'descNum'},
                                              }
                                   });
            }
        else {
            $("#matches").tablesorter( {sortList: slist,
                                    headers: {0: {sorter:'dates'},
                                    	      1: {sorter:false},	
                                              3: {sorter:'rounds'},
                                              //4: {sorter:'ranks'},
                                              //5: {sorter:'ranks'},
                                              4: {sorter:false}, // matchup
                                              5: {sorter:false}, // score -- more I can do with this?
                                              6: {sorter:'descNum'},
                                              7: {sorter:'descNum'},
                                              8: {sorter:'ascNum'},
                                              9: {sorter:'descNum'},
                                              10: {sorter:'descNum'},
                                              11: {sorter:'descNum'},
                                              12: {sorter:false},
                                              13: {sorter:'descNum'}
                                              }
                                   });
            }
        }
    var currentParams = getFilterParams();
    var opplist = [], oppteamlist = [], partnerlist = [];
    if (!$('#oppdef').hasClass('selected')) {var opplist = $('.oppselected').text().split(fourspaces + fourspaces).slice(1);}
    if (!$('#oppteamdef').hasClass('selected')) {var oppteamlist = $('.oppteamselected').text().split(fourspaces + fourspaces).slice(1);}
    if (!$('#partnerdef').hasClass('selected')) {var partnerlist = $('.partnerselected').text().split(fourspaces + fourspaces).slice(1);}
    var statrow = makeSplitStatRow(currentfilter);
    if ($mtbody.children().length == 0) {}
    else if (currentParams == '' && (opplist.length == 0 && oppteamlist.length == 0 && partnerlist.length == 0)) {}
    else {
        var rowid = '#f' + currentParams;
        //if (opplist.length == 0 && oppteamlist.length == 0 && partnerlist.length == 0) {var rowid = '#f' + currentParams;}
        if (opplist.length > 0) {
            var nots = $('.oppselected').text().replace(/\u00a0/g, '');
            rowid = rowid + '-' + nots;
            }
        if (oppteamlist.length > 0) {
            var oppteams = $('.oppteamselected').text().replace(/\u00a0/g, '');
            rowid = rowid + '-' + oppteams;
            }
        if (partnerlist.length > 0) {
            var partners = $('.partnerselected').text().replace(/\u00a0/g, '');
            rowid = rowid + '-' + partners;
            }
        if ($(rowid).length > 0) {$(rowid).remove();} // if same filter is already present, delete
        else if ($('.filtersplitrow').length == 5) {$('.filtersplitrow').last().remove();} // if 5 already, delete last

        var $filtrow = $('<tr>').attr('class', 'filtersplitrow')
        $filtrow.attr('id', rowid.slice(1));
        var ftitle = '';
        if (partnerlist.length == 0) {} // get label
        else {
            for (k=0; k<partnerlist.length; k++) {
                //var oppname = insertNameSpaces(opplist[k]);
                var ftitle = ftitle + 'Partner: ' + partnerlist[k] + '; '; // need to parse this for initials? last name?
                }
            ftitle = ftitle.slice(0, -2); // delete final semi-colon and space
            }
        if (opplist.length == 0) {} // get label
        else {
            for (k=0; k<opplist.length; k++) {
                //var oppname = insertNameSpaces(opplist[k]);
                var ftitle = ftitle + 'Opponent: ' + opplist[k] + '; '; // need to parse this for initials? last name?
                }
            ftitle = ftitle.slice(0, -2); // delete final semi-colon and space
            }
        if (oppteamlist.length == 0) {} // get label
        else {
            var mult_unit = unitePartners(oppteamlist);
            oppteamlist = mult_unit[1];
            for (k=0; k<oppteamlist.length; k++) {
                //var oppname = insertNameSpaces(opplist[k]);
                var ftitle = ftitle + 'Opp Team: ' + oppteamlist[k] + '; '; // need to parse this for initials? last name?
                }
            ftitle = ftitle.slice(0, -2); // delete final semi-colon and space
            }  
        ftitle = getFilterTitle(ftitle);      

        var $nameital = ($('<span>').css('font-style', 'italic').attr('class', 'filterlabel'));
        if (ftitle.length > 25) { // too long, replace end with ..., add title, etc.
            $nameital.attr('title', ftitle.replace(/; /g, '\n')).css('cursor', 'pointer').text(' ' + ftitle.slice(0,22) + '... ');
            }
        else {$nameital.text(' ' + ftitle + ' ');}
        
        var $closebutton = ($('<span>').text("[x]").attr('class', 'rowcloser likelink')) // add button to close
        //var $splitclick = $('<span>').text('(+)').attr('class', 'likelink splitclick');
        
        $filtrow.append($('<td>').append($closebutton).append($nameital)); //.append($splitclick));
        for (var i = 0; i < statrow.length; i++) {
            var $fcell = $('<td>').attr('align', 'right').text(statrow[i]);
            $filtrow.append($fcell);
            }
        $("#splitsbody").prepend($filtrow)

        $(".rowcloser").click(function () {// attach handler to .rowcloser
            $(this).parent().parent().remove();
            })
        }
        
    // add title above matches
    if (ftitle == "" || ftitle == undefined) {var ftitle = 'Last 52';}
    else {var tabtitle = 'Matches: '+ftitle;} //.replace(/; /g, ' > ');}
    var wlrec = '(' + statrow[0].slice(0,statrow[0].indexOf('(')-1) + ')';
    $("#tablelabel").html('<b>Matches '+wlrec+' > '+ftitle+'</b>');
        
    // how to count filters applied?
    if (currentParams == '' && (opplist.length == 0 && oppteamlist.length == 0 && partnerlist.length == 0)) {}
    else if ($mtbody.children().length == 0) {} // no matches -- keep this or not?
    else {
        var $bmatches = $('<span>').text('Apply filters to another player: ');
        $bmatches.append($('<input>').attr('id', 'paramsearch'));
        $('#matches').after($bmatches);
        }
    $(".h2hclick").click(function () {
        opponent = $(this).parent().attr('class');
        var withspaces = insertNameSpaces(opponent);
        setPlayerFilter(withspaces, 1, 0);
        if ($("#h2hhead").hasClass("closed")) {
            $("#h2hhead").removeClass("closed").addClass("open") //.addClass("selected");
            $(".h2hchoice").toggle();
            }
        })
    $(".menureset2").click(function () {
        resetFilters();
        // close open menus?
        makeMatchTable({}, 1);
        });
    $("#careerclick").click(function () {
        if ($("#spanhead").hasClass("closed")) {
            $("#spanhead").removeClass("closed").addClass("open") //.addClass("selected");
            $(".spanchoice").toggle();
            }
        $(".spanchoice").removeClass("selected").removeClass('spanselected');
        $("#spanCareerqq").addClass('selected').addClass('spanselected');
        makeMatchTable({}, 0);
        })
    }
    
function renderHeadToHeads(defSort) {
   
    var hdrs = hdrsHead;
    var slist = [[0,0]]; // for now, 2nd column, descending
    if (defSort == 0) {slist = confirmSort(slist);}
    
    var hDict = {}

    var $matchtable = $('<table>');
    var $mthead = $('<thead>');
    var $mtheaderrow = $('<tr>').attr('id', 'matchheader'); 
    var $mtfoot = $('<tfoot>');
    var $mtfooterrow = $('<tr>').attr('id', 'matchfooter').attr('background-color', '#FFF'); // color weirdness?  
    var $mtbody = $('<tbody>');
    var rightaligns = {0: '', 2: '', 3: '', 4: '', 5: '', 6: '', 7: '', 8: '', 9: '', 10: '', 
                       11: '', 12: '', 13: '', 14: '', 15: '', 16: '', 17: '', 18: '', 19: '', 20: '', 21: ''};
    for (var s = 0; s < matchmx.length; s++) {   
        if (s == 0) {
            // header row
            var cells = [];
            for (var i = 0; i < hdrs.length; i++) {
                var $mthspan = $('<span>').text(hdrs[i]);
                if (hdrs[i] in titleTips) {$mthspan.attr('title', titleTips[hdrs[i]]);}
                var $mthcell = $('<th>').append($mthspan);
                if (i in rightaligns) {$mthcell.attr('align', 'right');}
                $mtheaderrow.append($mthcell);
                }
            $mthead.append($mtheaderrow);
            $matchtable.append($mthead);
            // footer row -- do i want to keep this for head2head view?
            var fcells = [];
            for (var i = 0; i < hdrs.length; i++) {
                if (i == 1 && !$("#spanCareerqq").hasClass('selected')) { // && career is not selected
                    var $mftspan = $('<span>').text('Show Career').attr('font-style', 'italic');
                    $mftspan.attr('id', 'careerclick').attr('class', 'likelink');
                    }
                else {
                    var $mftspan = $('<span>').text('');
                    }
                var $mftcell = $('<td>').append($mftspan);
                $mtfooterrow.append($mftcell);
                }
            $mtfoot.append($mtfooterrow);
            $matchtable.append($mtfoot);
            }            
            
        var match = {} // create dict for each match
        for (var x=0; x<matchhead.length; x++) {
            if (matchmx[s].length <= x) {match[matchhead[x]] = '';}
            else {match[matchhead[x]] = matchmx[s][x];}
            }
            
        if (match.score == "" || match.score.slice(0,1) == 'D') {continue;}
        if (match.score.slice(0,1) == 'W' || match.score.slice(0,1) == 'R') {continue;}
        if (filterMatch(match, 'results') != 1) {continue;}
            
        match = getWinLossTiebreak(match);
                         
        if (!(match.opp in hDict)) {
            hDict[match.opp] = {'m': 0, 'w': 0, 'l': 0, 'occ': match.ocountry, 'ms': 0, 'dates': {}};
            for (u=0; u<stats.length; u++) {
                hDict[match.opp][stats[u]] = 0;
                }
            }
        hDict[match.opp]['m'] += 1
        if (match.wl == 'W') {hDict[match.opp]['w'] += 1;}
        else {hDict[match.opp]['l'] += 1;}
         
        //hDict[match.opp]['tiebreaks'] += match.tiebreaks
        //hDict[match.opp]['tbwon'] += match.tbwon
        
        if (match.oswon != "") {hDict[match.opp]['ms'] += 1;}
        
        for (u=0; u<stats.length; u++) {
            t = stats[u];
            if (match[t] != '') {hDict[match.opp][t] = hDict[match.opp][t] + parseInt(match[t]);}
            }        
        
        var wlfull = {'W': 'WIN', 'L': 'LOSS'};
        var details = wlfull[match.wl] + ': ' + match.matchid.slice(0,4) + ' ' + match.tourn + ' ' + match.round + ' (' + match.surf + '), ' + match.score;
        hDict[match.opp]['dates'][match.date] = details;
        }

    // check against match minimum (abbreviated version of genfilter())
    var multselect = $('.minimumselected').text().split(fourspaces + fourspaces).slice(1);
    var matchmin = multselect[0];
    if (matchmin == 'All') {var mm = 1;}
    else {var mm = parseInt(matchmin);}
        
    for (player in hDict) {
        if (!hDict.hasOwnProperty(player)) {continue;}
	if (hDict[player].m < mm) {continue;}
        
        var $mtrow = $('<tr>');
        
        var $deflink = $('<span>').text(hDict[player].m);
        $deflink.attr('class', 'likelink h2hclick');
        var $matchcell = $('<td>').append($deflink).attr('class', player.replace(/ /g, '')).attr('align', 'right');
        $mtrow.append($matchcell);

	$oppcc = $('<span>')
	$oppcc.text(' [' + hDict[player].occ + ']');
        var nlink = 'https://www.tennisabstract.com/cgi-bin/player.cgi?p=' + player.replace(/ /g, '') // opp link
        var $olk = $('<a>').text(player).attr('href', nlink);
        var $oppcell = $('<td>').append($olk).append($oppcc);     
        $mtrow.append($oppcell);
        
        // get list of h2h match dates
        var mdates = [];
        for (md in hDict[player]['dates']) {
            if (hDict[player]['dates'].hasOwnProperty(md)) {mdates.push(md);}
            }
	mdates.sort()
	
	$firstdate = $('<td>').text(showDate(mdates[0])).attr('align', 'right');
	$firstdate.attr('title', hDict[player]['dates'][mdates[0]]);
	$lastdate = $('<td>').text(showDate(mdates[mdates.length - 1])).attr('align', 'right');
	$lastdate.attr('title', hDict[player]['dates'][mdates[mdates.length - 1]]);     
        
        $mtrow.append($('<td>').text(hDict[player].w).attr('align', 'right'));
        $mtrow.append($('<td>').text(hDict[player].l).attr('align', 'right'));
        var wlperc = alignRound(hDict[player].w/hDict[player].m, 1, 1);
        $mtrow.append($('<td>').text(wlperc).attr('align', 'right'));
        
        $mtrow.append($('<td>').text(hDict[player].tiebreaks).attr('align', 'right'));
        $mtrow.append($('<td>').text(hDict[player].tbwon).attr('align', 'right'));
        $mtrow.append($('<td>').text(hDict[player].tiebreaks - hDict[player].tbwon).attr('align', 'right'));
        var tbperc = alignRound(hDict[player].tbwon/hDict[player].tiebreaks, 1, 1);
        $mtrow.append($('<td>').text(tbperc).attr('align', 'right'));
        
        $mtrow.append($firstdate);
        $mtrow.append($lastdate);
        
        $mtrow.append($('<td>').text(hDict[player].ms).attr('align', 'right')); // stat sample
        
        var rpw = 1 - (parseInt(hDict[player].ofwon) + parseInt(hDict[player].oswon))/hDict[player].opts; // dominance ratio:
        var spw = ((parseInt(hDict[player].fwon) + parseInt(hDict[player].swon))/hDict[player].pts);
        var spl = 1 - spw;
        var num = rpw/spl, dec = 2;
        var domratio = alignRound(rpw/spl, dec);

        var acerate = alignRound((hDict[player].aces/hDict[player].pts), 1, 1);
        var dfrate = alignRound((hDict[player].dfs/hDict[player].pts), 1, 1);
        var firstin = alignRound((hDict[player].firsts/hDict[player].pts), 1, 1);
        var fwin = alignRound((hDict[player].fwon/hDict[player].firsts), 1, 1);
        var swin = alignRound((hDict[player].swon/(hDict[player].pts-hDict[player].firsts)), 1, 1);
        var bksavedRate = alignRound((hDict[player].saved/hDict[player].chances), 1, 1)
        var bksaved = hDict[player].saved + '/' + hDict[player].chances;

        var pointswon = parseInt(hDict[player].fwon) + parseInt(hDict[player].swon) + (hDict[player].opts - hDict[player].ofwon - hDict[player].oswon);
        var tpw = alignRound((pointswon/(parseInt(hDict[player].pts) + parseInt(hDict[player].opts))), 1, 1);
        var rpw = alignRound(1 - ((parseInt(hDict[player].ofwon) + parseInt(hDict[player].oswon))/hDict[player].opts), 1, 1);
        var vace = alignRound((hDict[player].oaces/hDict[player].opts), 1, 1);
        var r1pw = alignRound(1 - (hDict[player].ofwon/hDict[player].ofirsts), 1, 1);
        var r2pw = alignRound(1 - (hDict[player].oswon/(hDict[player].opts-hDict[player].ofirsts)), 1, 1);
        var bkconvRate = alignRound(1 - (hDict[player].osaved/hDict[player].ochances), 1, 1)
        var bkconv = (hDict[player].ochances - hDict[player].osaved) + '/' + hDict[player].ochances;
        
        var statrow = [domratio, acerate, dfrate, firstin, fwin, swin, alignRound(spw, 1, 1), rpw]; 
        for (var i = 0; i < statrow.length; i++) {
            $mtrow.append($('<td>').text(statrow[i]).attr('align', 'right'));
            }        
	$bpsv = $('<td>').text(bksavedRate).attr('align', 'right');
	$bpsv.attr('title', bksaved);
	$bpcv = $('<td>').text(bkconvRate).attr('align', 'right');
	$bpcv.attr('title', bkconv);
        
        $mtrow.append($bpsv);
        $mtrow.append($bpcv);        
               
        $mtbody.append($mtrow);
        }

    // back to match table
    if ($mtbody.children().length == 0) {
        if (!$("#spanCareerqq").hasClass('selected')) { // career is not selected
            //var $mftspan = $('<span>').text('show career').attr('font-style', 'italic');
            //$mftspan.attr('id', 'careerclick').attr('class', 'likelink');
            var mft = '<span id="careerclick" class="likelink">Show career</span>, c';
            }
        else {var mft = 'C';}
        pTable = '<p id="matches">&nbsp;<br/>&nbsp;<br/>Your filters returned no matches. ' + mft + 'hange a filter or two, or press the '
        pTable = pTable + '<span class="menureset2 likelink">reset button</span>.</p>'
        $("#matches").replaceWith(pTable);
        }
    else {
        $matchtable.append($mtbody).attr('id', 'matches').attr('class', 'tablesorter');
        $matchtable.attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
        $("#stats").html($matchtable);
        $("#matches").tablesorter( {sortList: slist,
                                    headers: {1: {sorter:false}, // player
                                              9: {sorter:'dates'},
                                              10: {sorter:'datesDesc'},
                                    	      0: {sorter:'descNum'},	
                                              2: {sorter:'descNum'},
                                              3: {sorter:'descNum'},
                                              4: {sorter:'descNum'},
                                              5: {sorter:'descNum'}, 
                                              6: {sorter:'descNum'},
                                              7: {sorter:'descNum'},
                                              8: {sorter:'descNum'},
                                              11: {sorter:'descNum'},
                                              12: {sorter:'descNum'},
                                              13: {sorter:'descNum'},
                                              14: {sorter:'ascNum'},
                                              15: {sorter:'descNum'},
                                              16: {sorter:'descNum'},
                                              17: {sorter:'descNum'},
                                              18: {sorter:'descNum'},
                                              19: {sorter:'descNum'},
                                              20: {sorter:'descNum'},
                                              21: {sorter:'descNum'}                                             
                                              }
                                   });
        }
    
    var ftitle = getFilterTitle("");
    var pgtitle = '<b>Head-to-Head Records ('+ftitle+')</b>'; // <br/>Just testing blah blah blah.';
    $("#tablelabel").html(pgtitle);
    
    $(".h2hclick").click(function () {
        // switch back to results tab
        $(".tabview").addClass("tablink");
        $("#tabResults").removeClass("tablink");
        // unlike 'tabclick', don't reset any filters
        opponent = $(this).parent().attr('class');
        var withspaces = insertNameSpaces(opponent);
        setPlayerFilter(withspaces, 1, 1); // this includes makeMatchTable
        if ($("#h2hhead").hasClass("closed")) {
            $("#h2hhead").removeClass("closed").addClass("open") //.addClass("selected");
            $(".h2hchoice").toggle();
            }
        $(".header").show();
        $("#minimumhead").hide();            
        })
    }
    
function renderEvents(defSort) {
   
    var hdrs = hdrsEvents;
    var slist = [[0,0]]; // for now, 1st column; default is descending
    if (defSort == 0) {slist = confirmSort(slist);}
    
    var hDict = {}

    var $matchtable = $('<table>');
    var $mthead = $('<thead>');
    var $mtheaderrow = $('<tr>').attr('id', 'matchheader'); 
    var $mtfoot = $('<tfoot>');
    var $mtfooterrow = $('<tr>').attr('id', 'matchfooter').attr('background-color', '#FFF'); // color weirdness?  
    var $mtbody = $('<tbody>');
    var rightaligns = {0: '', 3: '', 4: '', 5: '', 6: '', 7: '', 8: '', 9: '', 10: '', 
                       11: '', 12: '', 13: '', 14: '', 15: '', 16: '', 17: '', 18: '', 19: '', 20: '', 21: '', 22: '', 23: '', 24: ''};
    for (var s = 0; s < matchmx.length; s++) {   
        if (s == 0) {
            // header row
            var cells = [];
            for (var i = 0; i < hdrs.length; i++) {
                var $mthspan = $('<span>').text(hdrs[i]);
                if (hdrs[i] in titleTips) {$mthspan.attr('title', titleTips[hdrs[i]]);}
                var $mthcell = $('<th>').append($mthspan);
                if (i in rightaligns) {$mthcell.attr('align', 'right');}
                $mtheaderrow.append($mthcell);
                }
            $mthead.append($mtheaderrow);
            $matchtable.append($mthead);
            // footer row -- do i want to keep this for head2head view?
            var fcells = [];
            for (var i = 0; i < hdrs.length; i++) {
                if (i == 1 && !$("#spanCareerqq").hasClass('selected')) { // && career is not selected
                    var $mftspan = $('<span>').text('Show Career').attr('font-style', 'italic');
                    $mftspan.attr('id', 'careerclick').attr('class', 'likelink');
                    }
                else {
                    var $mftspan = $('<span>').text('');
                    }
                var $mftcell = $('<td>').append($mftspan);
                $mtfooterrow.append($mftcell);
                }
            $mtfoot.append($mtfooterrow);
            $matchtable.append($mtfoot);
            }            
            
        var match = {} // create dict for each match
        for (var x=0; x<matchhead.length; x++) {
            if (matchmx[s].length <= x) {match[matchhead[x]] = '';}
            else {match[matchhead[x]] = matchmx[s][x];}
            }
        
        var tourlevel = {'A': '', 'M': '', 'G': ''};
        if (!(match.level in tourlevel)) {continue;}
        if (match.tourn.slice(0,4) == 'ATP ') {continue;}
        
        if (match.wl == 'W' && match.round != 'F') {  
            if (match.score == "" || match.score.slice(0,1) == 'D') {continue;}
            if (match.score.slice(0,1) == 'W' || match.score.slice(0,1) == 'R') {continue;}
            }
        
        if (filterMatch(match, 'results') != 1) {continue;}
            
        match = getWinLossTiebreak(match);
        
        if (match.tourn.indexOf('Olympics') > -1) {match.tourn = 'Olympics';}
                         
        if (!(match.tourn in hDict)) {
            hDict[match.tourn] = {'m': 0, 'w': 0, 'l': 0, 'occ': match.ocountry, 'ms': 0, 'dates': {}, 'surfs': {}, 'matches': {}};
            for (u=0; u<stats.length; u++) {
                hDict[match.tourn][stats[u]] = 0;
                }
            }
        hDict[match.tourn]['m'] += 1
        if (match.wl == 'W') {hDict[match.tourn]['w'] += 1;}
        else {hDict[match.tourn]['l'] += 1;}
        
        if (match.oswon != "") {hDict[match.tourn]['ms'] += 1;}
        
        for (u=0; u<stats.length; u++) {
            t = stats[u];
            if (match[t] != '') {hDict[match.tourn][t] = hDict[match.tourn][t] + parseInt(match[t]);}
            }        

        var myear;
        if (match.matchid.length > 0) {myear = match.matchid.slice(0,4);}
        else if (parseInt(match.date.slice(4,8)) > 1215) {myear = parseInt(match.date.slice(0,4)) + 1;}
        else {myear = match.date.slice(0,4);}
        
        hDict[match.tourn]['surfs'][myear] = match.surf;
        
        if (match.round == 'F' && match.wl == 'W') {
            hDict[match.tourn]['dates'][myear] = 'W';
            hDict[match.tourn]['matches'][myear] = 'WIN: '+match.round+' vs '+match.opp+', '+match.score;
            }
        else if (!(myear in hDict[match.tourn]['dates']) && match.wl == 'L') {
            hDict[match.tourn]['dates'][myear] = match.round;
            hDict[match.tourn]['matches'][myear] = 'LOSS: '+match.round+' vs '+match.opp+', '+match.score;
            }
        // make sure to get some date in there in case only wins in an incomplete tournament
        else if (!(myear in hDict[match.tourn]['dates']) && match.wl == 'W') {
            hDict[match.tourn]['dates'][myear] = "";
            hDict[match.tourn]['matches'][myear] = "";
            }
        // covers for previous line -- if year already in but only wins, get the loss
        else if ((myear in hDict[match.tourn]['dates'] && hDict[match.tourn]['dates'][myear] == "") && match.wl == 'L') {
            hDict[match.tourn]['dates'][myear] = match.round;
            hDict[match.tourn]['matches'][myear] = 'LOSS: '+match.round+' vs '+match.opp+', '+match.score;
            }
        }

    // check against match minimum (abbreviated version of genfilter())
    var multselect = $('.minimumselected').text().split(fourspaces + fourspaces).slice(1);
    var matchmin = multselect[0];
    if (matchmin == 'All') {var mm = 1;}
    else {var mm = parseInt(matchmin);}
    
    // check against match minimum (abbreviated version of genfilter())
    var multselectYears = $('.minyearsselected').text().split(fourspaces + fourspaces).slice(1);
    var yearsmin = multselectYears[0];
    if (yearsmin == 'All') {var mmy = 1;}
    else {var mmy = parseInt(yearsmin);}    
        
    for (event in hDict) {
        if (!hDict.hasOwnProperty(event)) {continue;}
	if (hDict[event].m < mm) {continue;}
        
        var $mtrow = $('<tr>');
        
        // get list of years
        var mdates = [];
        var allResults = {}
        for (md in hDict[event]['dates']) {
            if (hDict[event]['dates'].hasOwnProperty(md)) {
                mdates.push(md);
                allResults[hDict[event]['dates'][md]] = '';
                }
            }
            
        if (mdates.length < mmy) {continue;} // check against year minimum from filter
            
	mdates.sort();
	var lastyear = mdates[mdates.length-1];
	
	$mtrow.append($('<td>').text(mdates.length).attr('align', 'right'));     

	//$oppcc = $('<span>') // link to same event results?
	//$oppcc.text(' [' + hDict[player].occ + ']');
        //var nlink = 'https://www.tennisabstract.com/cgi-bin/player.cgi?p=' + player.replace(/ /g, '') // opp link
        //var $olk = $('<a>').text(player).attr('href', nlink);
        var $eventname = $('<span>').text(event).attr('class', 'likelink eventclick');
        var $oppcell = $('<td>').append($eventname).attr('class', event.replace(/ /g, '_'));     
        $mtrow.append($oppcell);
        
        $mtrow.append($('<td>').text(hDict[event]['surfs'][lastyear])); 
        
        var $deflink = $('<span>').text(hDict[event].m); // add link to event results? would need event version of h2hclick
        //$deflink.attr('class', 'likelink h2hclick');
        var $matchcell = $('<td>').append($deflink).attr('align', 'right') //.attr('class', player.replace(/ /g, ''))
        $mtrow.append($matchcell);        
	
	$firstdate = $('<td>').text(mdates[0]).attr('align', 'right');
	$firstdate.attr('title', hDict[event]['matches'][mdates[0]]);
	$lastdate = $('<td>').text(lastyear).attr('align', 'right');
	$lastdate.attr('title', hDict[event]['matches'][lastyear]);  
        
        $mtrow.append($('<td>').text(hDict[event].w).attr('align', 'right'));
        $mtrow.append($('<td>').text(hDict[event].l).attr('align', 'right'));
        var wlperc = alignRound(hDict[event].w/hDict[event].m, 1, 1);
        $mtrow.append($('<td>').text(wlperc).attr('align', 'right'));
        
        $mtrow.append($('<td>').text(hDict[event].tiebreaks).attr('align', 'right'));
        $mtrow.append($('<td>').text(hDict[event].tbwon).attr('align', 'right'));
        $mtrow.append($('<td>').text(hDict[event].tiebreaks - hDict[event].tbwon).attr('align', 'right'));
        var tbperc = alignRound(hDict[event].tbwon/hDict[event].tiebreaks, 1, 1);
        $mtrow.append($('<td>').text(tbperc).attr('align', 'right'));
        
        $mtrow.append($firstdate);
        $mtrow.append($lastdate);
        
        // get best result, with title for years achieved
        var rdpref = ['W', 'F', 'SF', 'QF', 'RR', 'R16', 'R32', 'R64', 'R128'];
        var bestResult = '';
        for (var i = 0; i < rdpref.length; i++) {
            if (rdpref[i] in allResults) {
                bestResult = rdpref[i];
                break;
                }
            }
        
        var bestYears = [];
        for (md in hDict[event]['dates']) {
            if (hDict[event]['dates'].hasOwnProperty(md)) {
                if (hDict[event]['dates'][md] == bestResult) {bestYears.push(md);}
                }
            } 
        bestYears.sort();    
        var bestText =  bestYears.join(',');
        
	$bestres = $('<td>').text(bestResult).attr('align', 'right');
	$bestres.attr('title', bestText);        
        $mtrow.append($bestres);       
        
        $mtrow.append($('<td>').text(hDict[event].ms).attr('align', 'right')); // stat sample
        
        var rpw = 1 - (parseInt(hDict[event].ofwon) + parseInt(hDict[event].oswon))/hDict[event].opts; // dominance ratio:
        var spw = ((parseInt(hDict[event].fwon) + parseInt(hDict[event].swon))/hDict[event].pts);
        var spl = 1 - spw;
        var num = rpw/spl, dec = 2;
        var domratio = alignRound(rpw/spl, dec);

        var acerate = alignRound((hDict[event].aces/hDict[event].pts), 1, 1);
        var dfrate = alignRound((hDict[event].dfs/hDict[event].pts), 1, 1);
        var firstin = alignRound((hDict[event].firsts/hDict[event].pts), 1, 1);
        var fwin = alignRound((hDict[event].fwon/hDict[event].firsts), 1, 1);
        var swin = alignRound((hDict[event].swon/(hDict[event].pts-hDict[event].firsts)), 1, 1);
        var bksavedRate = alignRound((hDict[event].saved/hDict[event].chances), 1, 1)
        var bksaved = hDict[event].saved + '/' + hDict[event].chances;

        var pointswon = parseInt(hDict[event].fwon) + parseInt(hDict[event].swon) + (hDict[event].opts - hDict[event].ofwon - hDict[event].oswon);
        var tpw = alignRound((pointswon/(parseInt(hDict[event].pts) + parseInt(hDict[event].opts))), 1, 1);
        var rpw = alignRound(1 - ((parseInt(hDict[event].ofwon) + parseInt(hDict[event].oswon))/hDict[event].opts), 1, 1);
        var vace = alignRound((hDict[event].oaces/hDict[event].opts), 1, 1);
        var r1pw = alignRound(1 - (hDict[event].ofwon/hDict[event].ofirsts), 1, 1);
        var r2pw = alignRound(1 - (hDict[event].oswon/(hDict[event].opts-hDict[event].ofirsts)), 1, 1);
        var bkconvRate = alignRound(1 - (hDict[event].osaved/hDict[event].ochances), 1, 1)
        var bkconv = (hDict[event].ochances - hDict[event].osaved) + '/' + hDict[event].ochances;
        
        var statrow = [domratio, acerate, dfrate, firstin, fwin, swin, alignRound(spw, 1, 1), rpw]; 
        for (var i = 0; i < statrow.length; i++) {
            $mtrow.append($('<td>').text(statrow[i]).attr('align', 'right'));
            }        
	$bpsv = $('<td>').text(bksavedRate).attr('align', 'right');
	$bpsv.attr('title', bksaved);
	$bpcv = $('<td>').text(bkconvRate).attr('align', 'right');
	$bpcv.attr('title', bkconv);
        
        $mtrow.append($bpsv);
        $mtrow.append($bpcv);        
               
        $mtbody.append($mtrow);
        }

    // back to match table
    if ($mtbody.children().length == 0) {
        if (!$("#spanCareerqq").hasClass('selected')) { // career is not selected
            //var $mftspan = $('<span>').text('show career').attr('font-style', 'italic');
            //$mftspan.attr('id', 'careerclick').attr('class', 'likelink');
            var mft = '<span id="careerclick" class="likelink">Show career</span>, c';
            }
        else {var mft = 'C';}
        pTable = '<p id="matches">&nbsp;<br/>&nbsp;<br/>Your filters returned no matches. ' + mft + 'hange a filter or two, or press the '
        pTable = pTable + '<span class="menureset2 likelink">reset button</span>.</p>'
        $("#matches").replaceWith(pTable);
        }
    else {
        $matchtable.append($mtbody).attr('id', 'matches').attr('class', 'tablesorter');
        $matchtable.attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
        $("#stats").html($matchtable);
        $("#matches").tablesorter( {sortList: slist,
                                    headers: {11: {sorter:'ascNum'},
                                              12: {sorter:'descNum'},
                                              13: {sorter:'roundsDesc'},
                                    	      0: {sorter:'descNum'},	
                                              24: {sorter:'descNum'},
                                              3: {sorter:'descNum'},
                                              4: {sorter:'descNum'},
                                              5: {sorter:'descNum'}, 
                                              6: {sorter:'descNum'},
                                              7: {sorter:'descNum'},
                                              8: {sorter:'descNum'},
                                              9: {sorter:'descNum'},
                                              22: {sorter:'descNum'},
                                              23: {sorter:'descNum'},
                                              10: {sorter:'descNum'},
                                              17: {sorter:'ascNum'},
                                              15: {sorter:'descNum'},
                                              16: {sorter:'descNum'},
                                              14: {sorter:'descNum'},
                                              18: {sorter:'descNum'},
                                              19: {sorter:'descNum'},
                                              20: {sorter:'descNum'},
                                              21: {sorter:'descNum'}                                             
                                              }
                                   });
        }
    
    var ftitle = getFilterTitle("");
    var pgtitle = '<b>Tour-Level Event Records ('+ftitle+')</b>'; // <br/>Just testing blah blah blah.';
    $("#tablelabel").html(pgtitle);
    
    $(".eventclick").click(function () {
        // switch back to results tab
        $(".tabview").addClass("tablink");
        $("#tabResults").removeClass("tablink");
        // unlike 'tabclick', don't reset any filters
        var ename = $(this).parent().attr('class');
        //var withspaces = insertNameSpaces(opponent);
        //setPlayerFilter(withspaces, 1, 1); // this includes makeMatchTable
        if ($("#tourneyhead").hasClass("closed")) {
            $("#tourneyhead").removeClass("closed").addClass("open") //.addClass("selected");
            $(".tourneychoice").show();
            }
        var eclass = '#tourney'+ename+'qq';
	$(".tourneychoice").removeClass("selected").removeClass("tourneyselected"); 
	$(eclass).addClass("selected").addClass("tourneyselected");       
        $(".header").show();
        $("#minimumhead").hide();
        $(".minimumchoice").hide();
        $("#minyearshead").hide();
        $(".minyearschoice").hide();
        makeMatchTable({}, 1);            
        })
    }

function makeMatchTable(options, defSort) {
    var servestats, stdscores;
    var rows = [], chunks = [];
    var stx = $.trim($(".spanselected").text());
    // checking to see whether we need to add more matches

    if (keep_loading == 1) {   
        if (view == "doubles") {
            if ((matchmx_dubs.length <= shortlist && stx != 'Last 52') && (stx != '2023' && stx != '2024')) {
                if (careerjs_dubs == 1) {matchmx_dubs = matchmx_dubs.concat(morematchmx_dubs);}   // some way to force this to load, even if waiting
                };
            }
        else {    
            if ((matchmx.length <= shortlist && stx != 'Last 52') && (stx != '2023' && stx != '2024')) {
                if (careerjs == 1) {matchmx = matchmx.concat(morematchmx);}   // some way to force this to load, even if waiting
                };
            }
        }
        
    if (view != "") { // url specifies view other than results
        $(".tabview").addClass('tablink');
        if (view == "h2h") {$("#tabHead").removeClass('tablink');}
        else if (view == "events") {$("#tabEvents").removeClass('tablink');}
        else if (view == "doubles") {$("#tabDubs").removeClass('tablink');}
        view = '';
        } 
     
    if (!$('#tabResults').hasClass("tablink")) {var tabview = "results";}
    else if (!$('#tabHead').hasClass("tablink")) {var tabview = "head";}
    else if (!$('#tabEvents').hasClass("tablink")) {var tabview = "events";}
    else if (!$('#tabDubs').hasClass("tablink")) {var tabview = "doubles";}
    
    if (tabview == "results" || tabview == "doubles") {
        $("#abovestats").show();
        
        if ('overall' in prefilters) {
            $(".stattab").addClass('likelink');
            $(".statso").removeClass('likelink');
            delete prefilters['overall'];
            }
        else if ('return' in prefilters) {
            $(".stattab").addClass('likelink');
            $(".statsr").removeClass('likelink');
            delete prefilters['return'];
            }
        else if ('raw' in prefilters) {
            $(".stattab").addClass('likelink');
            $(".statsw").removeClass('likelink');
            delete prefilters['raw'];
            }
        else if ('overview' in prefilters) {
            $(".stattab").addClass('likelink');
            $(".statsa").removeClass('likelink');
            delete prefilters['overview'];
            }

        var hdrs; 
        var showstats = ''        
        if (tabview == "doubles") {
            $(".revscore").hide();
            $(".statsa").show();
            $(".statspacer").show();
            if (!$(".statso").hasClass('likelink')) {
                showstats = 'o';
                hdrs = hdrsDoublesServe;
                }
            else if (!$(".statsr").hasClass('likelink')) {
                showstats = 'r';
                hdrs = hdrsDoublesReturn;
                }
            else if (!$(".statsw").hasClass('likelink')) {
                showstats = 'w';
                hdrs = hdrsDoublesRaw;
                }
            else if (!$(".statsa").hasClass('likelink')) {
                showstats = 'a';
                hdrs = hdrsDoubles;
                }
            }
        else {
            $(".revscore").show();
            $(".statsa").hide();
            $(".statspacer").hide();
            if ($(".revscore").text() == 'Reverse Loss Scores') {stdscores = true;}
            else if ($(".revscore").text() == 'Standard Scores') {stdscores = false;}
            else if ('reverse' in prefilters) {
                $(".revscore").html('Standard Scores');
                stdscores = false;
                }
            else {
                $(".revscore").html('Reverse Loss Scores');
                stdscores = true;
                }
            if (!$(".statso").hasClass('likelink')) {
                showstats = 'o';
                hdrs = hdrsServe;
                }
            else if (!$(".statsr").hasClass('likelink')) {
                showstats = 'r';
                hdrs = hdrsReturn;
                }
            else if (!$(".statsw").hasClass('likelink')) {
                showstats = 'w';
                hdrs = hdrsRaw;
                }
            else if (!$(".statsa").hasClass('likelink')) {
                $(".stattab").addClass('likelink');
                $(".statso").removeClass('likelink');
                showstats = 'o';
                hdrs = hdrsServe;
                }
            }
        }
    else {$("#abovestats").hide();}

    if (tabview == "results") {
        renderResults(stdscores, showstats, hdrs, defSort);
        }
    if (tabview == "head") {
        renderHeadToHeads(defSort);
        }  
    if (tabview == "events") {
        renderEvents(defSort);
        }     
    if (tabview == "doubles") {
        renderDoubles(stdscores, showstats, hdrs, defSort);
        }      

    $("#titleclick").click(function () {
        // reset all other filters?
        if ($("#spanhead").hasClass("closed")) {
            $("#spanhead").removeClass("closed").addClass("open") //.addClass("selected");
            $(".spanchoice").toggle();
            }
        $(".spanchoice").removeClass("selected").removeClass('spanselected');
        $("#spanCareerqq").addClass('selected').addClass('spanselected');
        if ($("#roundhead").hasClass("closed")) {
            $("#roundhead").removeClass("closed").addClass("open") //.addClass("selected");
            $(".roundchoice").toggle();
            }
        $(".roundchoice").removeClass("selected").removeClass('roundselected');
        $("#round0").addClass('selected').addClass('roundselected');
        makeMatchTable({}, 0);
        })
    var paramsearchbox = "Find player";		
    $("#paramsearch").val(paramsearchbox);
    $("#paramsearch").focus(function() {
        if ($(this).val() == paramsearchbox) {$(this).val("");}
        var urlparams = getPermalinkParams();
        $( "#paramsearch" ).autocomplete({
                source: playerlist,
                minLength: 2,
                select: function(e, ui) {
                    var playerselect = ui.item.value;
                    var player = playerselect.slice(4);
                    var mw = playerselect.slice(1,2);
                    if (mw == 'M') {
                        var playerurl = 'https://www.tennisabstract.com/cgi-bin/player.cgi?p=' + player.replace(/ /g, '') + '&f=' + urlparams;
                        }
                    else {
                        var playerurl = 'https://www.tennisabstract.com/cgi-bin/wplayer.cgi?p=' + player.replace(/ /g, '') + '&f=' + urlparams;
                        }                        
                    window.open(playerurl, "_self");
                    }
            });
        });
        
    var pparams = getPermalinkParams();
    var new_path = 'https://www.tennisabstract.com/cgi-bin/player-classic.cgi?p=' + fullname.replace(/ /g, '') 
    if (pparams != '') {new_path = new_path + '&f=' + pparams;}
    history.pushState( {
        new_text: pparams,
        slug: new_path  
        }, null, new_path);
    //    }
    }

function makeMenus() {
    $('#footer').empty();
    if (!$('#tabDubs').hasClass("tablink") || view == "doubles") {
        var doubles = 1;
        }
    else {doubles = 0;}
    var $str = $('<table>').attr('class', 'menus');
    // partner first
    $str.append(
                $('<tr>').attr('id', 'partnerhead').attr('class', 'header closed') 
                        .append($('<th>').text(fourspaces + '\u00a0\u00a0' + 'Partner')
                        )
                )
    $spanspaces = $('<span>').text(fourspaces + fourspaces);
    $spaninput = $('<span>').append($('<input>').attr('id', 'partnersearch').css('width', '80'));
    $str.append(
                $('<tr>').attr('id', 'partnersearchrow').attr('class', 'partnerchoice inmenu') 
                        .append($('<td>').append($spanspaces).append($spaninput)
                        )
                )
    $str.append(
                $('<tr>').attr('id', 'partnerdef').attr('class', 'partnerselected partnerchoice selected inmenu menudefault')
                        .append($('<td>').text(fourspaces + fourspaces + 'All')
                                         )
                )
    // opponent
    $str.append(
                $('<tr>').attr('id', 'opphead').attr('class', 'header closed') 
                        .append($('<th>').text(fourspaces + '\u00a0\u00a0' + 'Opponent')
                        )
                )
    $spanspaces = $('<span>').text(fourspaces + fourspaces);
    $spaninput = $('<span>').append($('<input>').attr('id', 'oppsearch').css('width', '80'));
    $str.append(
                $('<tr>').attr('id', 'oppsearchrow').attr('class', 'oppchoice inmenu') 
                        .append($('<td>').append($spanspaces).append($spaninput)
                        )
                )
    $str.append(
                $('<tr>').attr('id', 'oppdef').attr('class', 'oppselected oppchoice selected inmenu menudefault')
                        .append($('<td>').text(fourspaces + fourspaces + 'All')
                                         )
                )
    // opp team 
    $str.append(
                $('<tr>').attr('id', 'oppteamhead').attr('class', 'header closed') 
                        .append($('<th>').text(fourspaces + '\u00a0\u00a0' + 'Opp Team')
                        )
                )
    $spanspaces = $('<span>').text(fourspaces + fourspaces);
    $spaninput = $('<span>').append($('<input>').attr('id', 'oppteamsearch').css('width', '80'));
    $str.append(
                $('<tr>').attr('id', 'oppteamsearchrow').attr('class', 'oppteamchoice inmenu') 
                        .append($('<td>').append($spanspaces).append($spaninput)
                        )
                )
    $str.append(
                $('<tr>').attr('id', 'oppteamdef').attr('class', 'oppteamselected oppteamchoice selected inmenu menudefault')
                        .append($('<td>').text(fourspaces + fourspaces + 'All')
                                         )
                )
    // beginning of h2h menu
    $str.append(
                $('<tr>').attr('id', 'h2hhead').attr('class', 'header closed') 
                        .append($('<th>').text(fourspaces + '\u00a0\u00a0' + 'Head-to-Head')
                        )
                )
    $spanspaces = $('<span>').text(fourspaces + fourspaces);
    $spaninput = $('<span>').append($('<input>').attr('id', 'h2hsearch').css('width', '80'));
    $str.append(
                $('<tr>').attr('id', 'h2hsearchrow').attr('class', 'h2hchoice inmenu') 
                        .append($('<td>').append($spanspaces).append($spaninput)
                        )
                )
    $str.append(
                $('<tr>').attr('id', 'h2hdef').attr('class', 'h2hselected h2hchoice selected inmenu menudefault')
                        .append($('<td>').text(fourspaces + fourspaces + 'All')
                                         )
                )
    // beginning of 'not' menu
    $str.append(
                $('<tr>').attr('id', 'nothead').attr('class', 'header closed') 
                        .append($('<th>').text(fourspaces + '\u00a0\u00a0' + 'Exclude Opp')
                        )
                )
    $spanspaces = $('<span>').text(fourspaces + fourspaces);
    $spaninput = $('<span>').append($('<input>').attr('id', 'notsearch').css('width', '80'));
    $str.append(
                $('<tr>').attr('id', 'notsearchrow').attr('class', 'notchoice inmenu') 
                        .append($('<td>').append($spanspaces).append($spaninput)
                        )
                )
    $str.append(
                $('<tr>').attr('id', 'notdef').attr('class', 'notselected notchoice selected inmenu menudefault')
                        .append($('<td>').text(fourspaces + fourspaces + 'None')
                                         )
                )
    for (j=0; j<filterlist.length; j++) {
        var filter = filterlist[j];
        var ftitle = filteropts[filter][0];
        var fhead = filter + 'head', fchoice = filter + 'choice', fselected = filter + 'selected';
        $str.append(
                    $('<tr>').attr('id', fhead).attr('class', 'header closed') 
                            .append($('<th>').text(fourspaces + '\u00a0\u00a0' + ftitle)
                            )
                    )
        $str.append(
                    $('<tr>').attr('id', filter + 'def').attr('class', fselected + ' ' + fchoice + ' selected inmenu menudefault')
                            .append($('<td>').text(fourspaces + fourspaces + filteropts[filter][1])
                                             )
                    )
        var flist = filteropts[filter].slice(2);
        for (i=0; i<flist.length; i++) {
            var $ent = $('<tr>').attr('class', 'inmenu ' + fchoice)
                                .append($('<td>').text(fourspaces + fourspaces + flist[i].replace(/ /g, '\u00a0'))
                                                 );
            if (filter in alphaFilters) {
                var adjtext = flist[i].replace(/ /g, '_');
                adjtext = adjtext.replace("'", 'xx');
                adjtext = adjtext.replace("+", 'xpx');
                $ent.attr('id', filter+adjtext+'qq');
                }
            else {$ent.attr('id', filter + i);}
            $str.append($ent);
            }
        //attempt at custom date ranges
        if (filter == 'span' || filter == 'rank') {
            if (filter == 'span' && (typeof tdates === 'undefined')) {}
            else if (filter == 'rank' && (typeof vranks === 'undefined')) {}
            else {
                var $ent = $('<tr>').attr('class', 'inmenu '+filter+'choice')
                                .append($('<td>').text(fourspaces + fourspaces + 'Custom')
                                                 );
                $ent.attr('id', filter+'Customqq');
                $str.append($ent);  
                }        
            }
        }
    $str.append($('<tr>').append($('<th>').append($('<span>').attr('class', 'menureset likelink').text('Reset'))))
    $str.append($('<tr>').append($('<td>').append($('<span>').html('&nbsp;'))))
    var filternote = '<i>Tip: Ctrl-Click to select multiple choices from the same menu.</i>'
    $str.append($('<tr>').append($('<td>').append($('<span>').html(filternote))))
    $('#footer').append($str);
    
    $("#h2hhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".h2hchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        if (choiceid != 'h2hsearchrow') {choiceClick(choiceid, 'h2h', ctrl);}
        });
    $("#nothead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".notchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        if (choiceid != 'notsearchrow') {choiceClick(choiceid, 'not', ctrl);}
        });
    $("#partnerhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".partnerchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        if (choiceid != 'partnersearchrow') {choiceClick(choiceid, 'partner', ctrl);}
        });
    $("#opphead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".oppchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        if (choiceid != 'oppsearchrow') {choiceClick(choiceid, 'opp', ctrl);}
        });
    $("#oppteamhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".oppteamchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        if (choiceid != 'oppteamsearchrow') {choiceClick(choiceid, 'oppteam', ctrl);}
        });
    $("#spanhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });       
    $(".spanchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'span', ctrl);
        });
    
    $("#surfacehead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".surfacechoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'surface', ctrl);
        });
    $("#levelhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".levelchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'level', ctrl);
        });
    $("#tourneyhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".tourneychoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'tourney', ctrl);
        });
    $("#roundhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".roundchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'round', ctrl);
        });
    $("#entryhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".entrychoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'entry', ctrl);
        });
    $("#rankhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".rankchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'rank', ctrl);
        });
    $("#oentryhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".oentrychoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'oentry', ctrl);
        });
    $("#handhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".handchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'hand', ctrl);
        });
    $("#handshead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".handschoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'hands', ctrl);
        });
    $("#agehead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".agechoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'age', ctrl);
        });
    $("#heighthead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".heightchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'height', ctrl);
        });
    $("#countryhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".countrychoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'country', ctrl);
        });
    $("#resultshead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".resultschoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'results', ctrl);
        });
    $("#setshead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".setschoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'sets', ctrl);
        });
    $("#scoreshead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".scoreschoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'scores', ctrl);
        });
    $("#asrankhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".asrankchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'asrank', ctrl);
        });
    $("#statshead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $("#statsdef").click(function (event) { // if creating custom stat filters to later click again, need to change this to match other filters: .statschoice
        if (event.ctrlKey) {var ctrl=1;} // probably disable
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'stats', ctrl);
        });
    $("#extrashead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".extraschoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'extras', ctrl);
        });
    $("#minimumhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".minimumchoice").click(function (event) {
        //if (event.ctrlKey) {var ctrl=1;}
        //else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'minimum', 0); // only allow one selection
        }); 
    $("#minyearshead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".minyearschoice").click(function (event) {
        //if (event.ctrlKey) {var ctrl=1;}
        //else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'minyears', 0); // only allow one selection
        });         
    $("#crankhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".crankchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'crank', ctrl); // only allow one selection
        });  
    $("#prankhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".prankchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'prank', ctrl); // only allow one selection
        }); 
    $("#phandhead").click(function () {
        var choiceid = $(this).attr('id');
        fHeaderClick(choiceid);
        });
    $(".phandchoice").click(function (event) {
        if (event.ctrlKey) {var ctrl=1;}
        else {var ctrl=0;}
        var choiceid = $(this).attr('id')
        choiceClick(choiceid, 'phand', ctrl); // only allow one selection
        });               
    $("#spanCustomqq").click(function (event) {
        $("#spanstartrow").remove();
        // smarter way -- check if it exists, then build if not?
        var $s = $('<select />').attr('id', 'spanstart');
        $('<option />', {value: 'default', text: 'Start'}).appendTo($s);
        for (i=0; i<tdates.length; i++) {
            var showdate = tdates[i].slice(6) + '-' + months[tdates[i].slice(4,6)] + '-' + tdates[i].slice(0,4);
            $('<option />', {value: tdates[i], text: showdate}).appendTo($s);
            }
        $sspan = $('<span>').text(fourspaces + fourspaces);
        $std = $('<td>').append($sspan).append($s)
        $startrow = $('<tr>').attr('class', 'inmenu spanchoice').attr('id', 'spanstartrow').append($std);                                                     
        $("#spanCustomqq").after($startrow);
        $startrow.toggle();
        $('#spanstart').change(function() {
            $("#spanendrow").remove();
            startdate = $("#spanstart option:selected").val();
            var $t = $('<select />').attr('id', 'spanend');
            $('<option />', {value: 'default', text: 'End'}).appendTo($t);
            for (i=0; i<tdates.length; i++) {
                if (tdates[i] < startdate) {}
                else {
                    var showdate = tdates[i].slice(6) + '-' + months[tdates[i].slice(4,6)] + '-' + tdates[i].slice(0,4);
                    $('<option />', {value: tdates[i], text: showdate}).appendTo($t);
                    }
                }
            $espan = $('<span>').text(fourspaces + fourspaces);
            $etd = $('<td>').append($espan).append($t)    
            $endrow = $('<tr>').attr('class', 'inmenu spanchoice').attr('id', 'spanendrow').append($etd);
            $("#spanstartrow").after($endrow);
            $endrow.toggle();  
            $('#spanend').change(function() {
                //alert('end is changed');
                enddate = $("#spanend option:selected").val();
                makeMatchTable({}, 0);
                });      
            });
        });
    $("#rankCustomqq").click(function (event) {
        $("#rankstartrow").remove();
        // smarter way -- check if it exists, then build if not?
        var $s = $('<select />').attr('id', 'rankstart');
        $('<option />', {value: 'default', text: 'Max'}).appendTo($s);
        for (i=0; i<vranks.length; i++) {
            $('<option />', {value: vranks[i], text: vranks[i]}).appendTo($s);
            }
        $srank = $('<rank>').text(fourspaces + fourspaces);
        $srd = $('<td>').append($srank).append($s)
        $lowrow = $('<tr>').attr('class', 'inmenu rankchoice').attr('id', 'rankstartrow').append($srd);                                                     
        $("#rankCustomqq").after($lowrow);
        $lowrow.toggle();
        $('#rankstart').change(function() {
            $("#rankendrow").remove();
            lowrank = parseInt($("#rankstart option:selected").val());
            var $t = $('<select />').attr('id', 'rankend');
            $('<option />', {value: 'default', text: 'Min'}).appendTo($t);
            for (i=0; i<vranks.length; i++) {
                if (parseInt(vranks[i]) < lowrank) {}
                else {
                    $('<option />', {value: vranks[i], text: vranks[i]}).appendTo($t);
                    }
                }
            $erank = $('<rank>').text(fourspaces + fourspaces);
            $erd = $('<td>').append($erank).append($t)    
            $highrow = $('<tr>').attr('class', 'inmenu rankchoice').attr('id', 'rankendrow').append($erd);
            $("#rankstartrow").after($highrow);
            $highrow.toggle();  
            $('#rankend').change(function() {
                //alert('end is changed');
                highrank = parseInt($("#rankend option:selected").val());
                makeMatchTable({}, 0);
                });      
            });
        });
    $(".menureset").click(function () {
        resetFilters();
        // close open menus?
        makeMatchTable({}, 1);
        });
        
    var h2hsearchbox = "Add H2H";		
    $("#h2hsearch").val(h2hsearchbox);
    $("#h2hsearch").focus(function() {
        if ($(this).val() != '') {$(this).val("");}
        $( "#h2hsearch" ).autocomplete({
                source: ochoices,
                minLength: 2,
                select: function(e, ui) {setPlayerFilter(ui.item.value, 1, 0);}
            }) 
        });
    var notsearchbox = "Add NOT";		
    $("#notsearch").val(notsearchbox);
    $("#notsearch").focus(function() {
        if ($(this).val() != '') {$(this).val("");}
        $( "#notsearch" ).autocomplete({
                source: ochoices,
                minLength: 2,
                select: function(e, ui) {setNotPlayerFilter(ui.item.value, 1);}
            }) 
        });
    var partnersearchbox = "Add Partner";		
    $("#partnersearch").val(partnersearchbox);
    $("#partnersearch").focus(function() {
        if ($(this).val() != '') {$(this).val("");}
        $( "#partnersearch" ).autocomplete({
                source: pchoices,
                minLength: 2,
                select: function(e, ui) {setPartnerFilter(ui.item.value, 1, 0);}
            }) 
        });
    var oppsearchbox = "Add Opponent";		
    $("#oppsearch").val(oppsearchbox);
    $("#oppsearch").focus(function() {
        if ($(this).val() != '') {$(this).val("");}
        $( "#oppsearch" ).autocomplete({
                source: ochoices_dubs,
                minLength: 2,
                select: function(e, ui) {setOpponentFilter(ui.item.value, 1, 0);}
            }) 
        });
    var oppteamsearchbox = "Add Opp Team";		
    $("#oppteamsearch").val(oppteamsearchbox);
    $("#oppteamsearch").focus(function() {
        if ($(this).val() != '') {$(this).val("");}
        $( "#oppteamsearch" ).autocomplete({
                source: otchoices,
                minLength: 2,
                select: function(e, ui) {setOppteamFilter(ui.item.value, 1, 0);}
            }) 
        });
        
    var $s = $('<select />').attr('id', 'statcustomname');
    $('<option />', {value: 'default', text: 'Stat'}).appendTo($s);
    for (i=0; i<statlist.length; i++) {
        $('<option />', {value: statlist[i], text: statlist[i]}).appendTo($s);
        }
    $sspan = $('<span>').text(fourspaces + fourspaces);
    $std = $('<td>').append($sspan).append($s)
    $startrow = $('<tr>').attr('class', 'inmenu statschoice').attr('id', 'stats0').append($std);                                                     
    $('#statsdef').after($startrow);
    $('#statcustomname').change(function() {
        $("#stats0").addClass("selected").addClass("statsselected");
        $("#statsdef").removeClass("selected").removeClass("statsselected");        
        $("#statoperatorrow").remove();
        $("#statinputrow").remove();
        $("#statbuttonrow").remove(); 
        statchoice = $("#statcustomname option:selected").val();
        var $t = $('<select />').attr('id', 'statoperator');
        $('<option />', {value: 'default', text: 'Operator'}).appendTo($t);
        $('<option />', {value: 'ge', text: '>= at least'}).appendTo($t);
        $('<option />', {value: 'le', text: '<= up to'}).appendTo($t);
        $('<option />', {value: 'gt', text: '> more than'}).appendTo($t);
        $('<option />', {value: 'lt', text: '< less than'}).appendTo($t);
        $('<option />', {value: 'eq', text: '= equal to'}).appendTo($t);
        $espan = $('<span>').text(fourspaces + fourspaces);
        $etd = $('<td>').append($espan).append($t)    
        $endrow = $('<tr>').attr('class', 'inmenu statschoice').attr('id', 'statoperatorrow').append($etd);
        $("#stats0").after($endrow);
        $endrow.toggle();  
        $('#statoperatorrow').change(function() {
            $("#statinputrow").remove();
            $("#statbuttonrow").remove();
            statoperatorchoice = $("#statoperator option:selected").val();
            // text box for manual entry
            var $v = $('<input />').attr('type', 'text').attr('id', 'statinputbox').attr('value', '').attr('style', "width:50px;"); 
            $ispan = $('<span>').text(fourspaces + fourspaces);
            if (statchoice in statpercs) {
                $perc = $('<span>').text(' %');
                $itd = $('<td>').append($ispan).append($v).append($perc);
                }
            else if (statchoice == 'Time') {
                $timeEx = $('<span>').text(' (h:mm)');
                $itd = $('<td>').append($ispan).append($v).append($timeEx);
                }           
            else {$itd = $('<td>').append($ispan).append($v);}
            $inputrow = $('<tr>').attr('class', 'inmenu statschoice').attr('id', 'statinputrow').append($itd);
            $("#statoperatorrow").after($inputrow);
            $inputrow.toggle();                     
            // button 'apply'
            var $w = $('<input />').attr('type', 'submit').attr('value', 'Apply').attr('style', "width:50px;").attr('id', 'statapply');
            $bspan = $('<span>').text(fourspaces + fourspaces);
            $btd = $('<td>').append($bspan).append($w);
            $buttonrow = $('<tr>').attr('class', 'inmenu statschoice').attr('id', 'statbuttonrow').append($btd);
            $("#statinputrow").after($buttonrow);
            $buttonrow.toggle(); 
            $("#statapply").click(function (event) {
                statinput = $("#statinputbox").val();
                if (statchoice == 'Time') {
                    var hm = statinput.split(':');
                    // probably need some error handling for user input, esp e.g. :50 without hour number
                    statinput = parseInt(hm[0])*60 + parseInt(hm[1]);
                    }  
                makeMatchTable({}, 0);             
                });
            });
        });
    }

var alphaFilters = {'country': '', 'tourney': '', 'span': '', 'rank': ''};
//var alphaFilters = {};

var bhdict = {'1': 'one', '2': 'two'};

function insertPictureBio() {
    var plname = '<span style="fontsize:24px"><b>' + fullname + ' [' + country + ']</b></span>';
    if (typeof dob === 'undefined') {var birthdate = '';}
    else {
        var bd = dob.toString();
        var birthdate = 'Date of birth: ' + bd.slice(6) + '-' + months[bd.slice(4, 6)] + '-' + bd.slice(0, 4);
        }

    var pic_src = 'https://www.tennisabstract.com/photos/' + fullname.toLowerCase().replace(/ /g, "_") + '-' + photog + '.jpg'
    // background-color:d3d3d3; 
    var biotable = '<p id="biog"><table style="border-spacing:0"><tr><td><img src="' + pic_src + '" height="225" width="150"></td>'
    biotable = biotable + '<td>&nbsp;&nbsp;</td><td style="vertical-align:top">'
    biotable = biotable + '<table cellpadding=2 cellspacing=1><tr><td>' + plname + fourspaces + fourspaces + '</td></tr>';  

    if (typeof twitter != 'undefined' && twitter != "") {
        var tline = '<a href="https://twitter.com/' + twitter + '" target="_blank">@' + twitter + '</a>';
        biotable = biotable + '<tr><td>' + tline + '</td></tr>';
        }
    else {
        biotable = biotable + '<tr><td>&nbsp;</td></tr>'; 
        }

    biotable = biotable + '<tr><td>' + birthdate + '</td></tr>';
    var handed;
    if (hand == 'R') {handed = 'Plays: Right-handed';}
    else if (hand == 'L') {handed = 'Plays: Left-handed';}
    else {handed = '';}
    if (typeof backhand != 'undefined' && backhand != '') {handed = handed.slice(0,-7) + ' (' + bhdict[backhand] + '-handed backhand)';}
    if (handed != '') {
        biotable = biotable + '<tr><td>' + handed + '</td></tr>'; 
        }

    if (currentrank == '') {var crank = '';}
    else {var crank = 'Current rank: <b>'+currentrank.toString()+'</b>'}; 
    //if (typeof liverank != 'undefined' && liverank != '') {var lrank = '&nbsp;&nbsp;&nbsp;<a target="_blank" href="https://live-tennis.eu/"><i>Live: ' + liverank.toString() + '</i></a>';}
    //else {var lrank = '';} 
    var lrank = '';
    if (crank != '') {
        biotable = biotable + '<tr><td>' + crank + lrank + '</td></tr>'; 
        }  

    if (peakrank != 'UNR') {var prank = 'Peak rank: <b>' + peakrank.toString()+'</b>';}
    else {var prank = '';}
    if (peakfirst != '') {
    	var pf = peakfirst.toString();
    	prank = prank + ' (' + pf.slice(6) + '-' + months[pf.slice(4, 6)] + '-' + pf.slice(0, 4) + ')';
    	}
    if (prank != '') {
        biotable = biotable + '<tr><td>' + prank + '</td></tr>'; 
        } 
        
    // if (typeof current_dubs === 'undefined' || current_dubs == "" || current_dubs == "UNR") {
    //     if (!(typeof peak_dubs === 'undefined' || peak_dubs == "")) {
    //         var drank = "Doubles peak: <b>" + peak_dubs.toString() + '</b>'
    //         var pfd = peakfirst_dubs.toString();
    //         drank = drank + ' (' + pfd.slice(6) + '-' + months[pfd.slice(4, 6)] + '-' + pfd.slice(0, 4) + ')';
    //         }
    //     else {drank = "&nbsp;";}
    //     }
    // else {
    //     var drank = "Doubles rank: <b>" + current_dubs.toString() + '</b>&nbsp;|&nbsp;';
    //     drank = drank + 'Peak: <b>' + peak_dubs.toString() + '</b>'
    //     }
    var drank = "&nbsp;";
    biotable = biotable + '<tr><td>' + drank + '</td></tr>'; 
        
    var profiles = '';
    // if (typeof atp_id != 'undefined' && atp_id != "") {
    //     var atp_stem = "https://www.atpworldtour.com/en/players/";
    //     profiles = profiles + '<a href="' + atp_stem + atp_id + '/overview" target="_blank" title="Profile at atpworldtour.com">ATP</a> | ';
    //     }   
    // if (typeof itf_id != 'undefined' && itf_id != "") {
    //     var itf_stem = "https://www.itftennis.com/procircuit/players/player/profile.aspx?playerid=";
    //     profiles = profiles + '<a href="' + itf_stem + itf_id + '" target="_blank" title="Profile at the ITF website">ITF</a> | ';
    //     }
    // if (typeof dc_id != 'undefined' && dc_id != "") {
    //     var dc_stem = "https://www.daviscup.com/en/players/player.aspx?id=";
    //     profiles = profiles + '<a href="' + dc_stem + dc_id + '" target="_blank" title="Profile at Davis Cup website">DC</a> | ';
    //     }
    if (typeof wiki_id != 'undefined' && wiki_id != "") {
        var wiki_stem = "https://en.wikipedia.org/wiki/";
        profiles = profiles + '<a href="' + wiki_stem + wiki_id + '" target="_blank" title="Profile at Wikipedia">Wikipedia</a> | '  ;
        }
        
    if (profiles != '') {
        biotable = biotable + '<tr><td>Profile: ' + profiles.slice(0,-3) + '</td><td>&nbsp;</td></tr>';
        } 
    else {
        biotable = biotable + '<tr><td>&nbsp;</td></tr>'; 
        } 

    // if picture (and maybe in general) need to move ranking history link and titles/finals out of this box
    //var rhlink = '<a href="https://tennisabstract.herokuapp.com/ATP/RankHistory/' + fullname.replace(/ /g, "_") + '/YearEndDate/Points/">Ranking history and highlights</a>';
    var titlelink = '<span id="titleclick" class="likelink">Titles/Finals</span>';
    //biotable = biotable + '<tr><td>' + rhlink + '</td></tr><tr><td>' + titlelink + '</td></tr>';
    biotable = biotable + '<tr><td>' + titlelink + '</td></tr>';

    biotable = biotable + '<tr><td>' + '<i>Photo: <a target="_blank" href="' + photog_link + '">' + photog_credit + '</a></i></td></tr>';

    $("#bio").html(biotable + '</table></td><td>&nbsp;&nbsp;</td></tr></table></p>');
    }

function insertBio() {
    var plname = '<span style="fontsize:24px"><b>' + fullname + ' [' + country + ']</b></span>';
    if (typeof dob === 'undefined') {var birthdate = '';}
    else {
        var bd = dob.toString();
        var birthdate = 'Date of birth: ' + bd.slice(6) + '-' + months[bd.slice(4, 6)] + '-' + bd.slice(0, 4);
        }
    var handed;
    if (hand == 'R') {handed = 'Plays: Right-handed';}
    else if (hand == 'L') {handed = 'Plays: Left-handed';}
    else {handed = '';}
    if (typeof backhand != 'undefined' && backhand != '') {handed = handed.slice(0,-7) + ' (' + bhdict[backhand] + '-handed backhand)';}
    if (currentrank == '') {var crank = '';}
    else {var crank = 'Current rank: <b>'+currentrank.toString()+'</b>'}; 
    // if (typeof liverank != 'undefined' && liverank != '') {var lrank = '&nbsp;&nbsp;&nbsp;<a target="_blank" href="https://live-tennis.eu/"><i>Live: ' + liverank.toString() + '</i></a>';}
    // else {var lrank = '';}
    var lrank = '';
    if (peakrank != 'UNR') {var prank = 'Peak rank: <b>' + peakrank.toString()+'</b>';}
    else {var prank = '';}
    if (peakfirst != '') {
    	var pf = peakfirst.toString();
    	prank = prank + ' (' + pf.slice(6) + '-' + months[pf.slice(4, 6)] + '-' + pf.slice(0, 4) + ')';
    	}
    //if (typeof twitter != 'undefined' && twitter != "") {var tline = 'Twitter: <a href="https://twitter.com/' + twitter + '" target="_blank">@' + twitter + '</a>';}
    if (typeof twitter != 'undefined' && twitter != "") {var tline = '<a href="https://twitter.com/' + twitter + '" target="_blank">@' + twitter + '</a>';}
    else {var tline = '';}
    var biotable = '<p id="biog"><table cellpadding=1 cellspacing=1><tr><td>' + plname + fourspaces + fourspaces + '</td><td>' + tline + '</td></tr>';
    if (birthdate != '') {var rowtwo = '<tr><td>' + birthdate + fourspaces + fourspaces + '</td><td>' + handed + '</td></tr>';}
    else {var rowtwo = '<tr><td>' + handed + '</td><td>&nbsp;</td></tr>';}
    if (crank != '') {var rowthree = '<tr><td>' + crank + lrank + '</td><td>' + prank + '</td></tr>';}
    else {var rowthree = '<tr><td>' + prank + '</td><td>&nbsp;</td></tr>';}
    
    var profiles = '';
    // if (typeof atp_id != 'undefined' && atp_id != "") {
    //     var atp_stem = "https://www.atpworldtour.com/en/players/";
    //     profiles = profiles + '<a href="' + atp_stem + atp_id + '/overview" target="_blank" title="Profile at atpworldtour.com">ATP</a> | ';
    //     }   
    // if (typeof itf_id != 'undefined' && itf_id != "") {
    //     var itf_stem = "https://www.itftennis.com/procircuit/players/player/profile.aspx?playerid=";
    //     profiles = profiles + '<a href="' + itf_stem + itf_id + '" target="_blank" title="Profile at the ITF website">ITF</a> | ';
    //     }
    // if (typeof dc_id != 'undefined' && dc_id != "") {
    //     var dc_stem = "https://www.daviscup.com/en/players/player.aspx?id=";
    //     profiles = profiles + '<a href="' + dc_stem + dc_id + '" target="_blank" title="Profile at Davis Cup website">DC</a> | ';
    //     }
    if (typeof wiki_id != 'undefined' && wiki_id != "") {
        var wiki_stem = "https://en.wikipedia.org/wiki/";
        profiles = profiles + '<a href="' + wiki_stem + wiki_id + '" target="_blank" title="Profile at Wikipedia">Wikipedia</a> | '  ;
        }
        
    if (profiles != '') {
        var profile_row = '<tr><td>Profile: ' + profiles.slice(0,-3) + '</td><td>&nbsp;</td></tr>';
        } 
    else {var profile_row = '';}      

    var rhlink = '&nbsp;' // '<a href="https://tennisabstract.herokuapp.com/ATP/RankHistory/' + fullname.replace(/ /g, "_") + '/YearEndDate/Points/">Ranking history and highlights</a>';
    var titlelink = '<span id="titleclick" class="likelink">Titles/Finals</span>'
    var rowfour =  '<tr><td>' + titlelink + '</td><td>' + rhlink + '</td></tr>'; 
    $("#bio").html(biotable + rowtwo + rowthree + profile_row + rowfour + '</table></p>');
    }

function choiceClick(choiceid, filt, ctrl) {
    //var menudef = 0;
    // if click is on default -- shift doesn't matter, presets don't matter; select default and deselect others
    // (possible exception of 'span' menu?
    if (choiceid.slice(-3) == 'def') {
        $("." + filt + "choice").removeClass("selected").removeClass(filt + "selected");
        $('#' + choiceid).addClass("selected").addClass(filt + "selected");
        }
    // if click on choice that is already selected [shift doesn't matter]
    else if ($('#' + choiceid).hasClass('selected')) {
        // - if it's the only one, go back to default
        if ($('.' + filt + 'selected').length == 1) {
            $('#' + filt + 'def').addClass("selected").addClass(filt + "selected");
            }
        $('#' + choiceid).removeClass("selected").removeClass(filt + "selected");
        }
    // if choice not already selected, and shift, then select new choice
    else if (ctrl == 1) {
        $('#' + choiceid).addClass("selected").addClass(filt + "selected");
        }
    // choice not already selected, not shift: take away all, make this selection
    else {
        $("." + filt + "choice").removeClass("selected").removeClass(filt + "selected");
        $('#' + choiceid).addClass("selected").addClass(filt + "selected");
        }
    if (choiceid.slice(-8) == 'Customqq') {return;}
    makeMatchTable({}, 0);
    }

function setPlayerFilter(player, make, defSort) {
    $('.h2hchoice').removeClass('selected').removeClass('h2hselected');
    var playerid = '#' + 'h2h' + player.replace(/ /g, '');
    if ($(playerid).length == 0) { // new player to the list
        $newcell = $('<td>').text(fourspaces + fourspaces + player.replace(/ /g, '\u00a0'));
        $newrow = $('<tr>').attr('class', 'h2hselected h2hchoice selected inmenu').attr('id', playerid.slice(1));
        $newrow.append($newcell);
        $('#nothead').before($newrow);
        if ($('#h2hhead').hasClass('open')) {$newrow.toggle();}
        $(playerid).click(function () {
            //var choiceid = $(this).attr('id')
            if (event.ctrlKey) {var ctrl=1;}
            else {var ctrl=0;}
            choiceClick(playerid.slice(1), 'h2h', ctrl);
            });
        }
    else {$(playerid).addClass('selected').addClass('h2hselected');}
    //set time span to career
    if (make==1) {makeMatchTable({}, defSort);}
    }
    
function setPartnerFilter(player, make, defSort) {
    $('.partnerchoice').removeClass('selected').removeClass('partnerselected');
    var playerid = '#' + 'partner' + player.replace(/ /g, '');
    if ($(playerid).length == 0) { // new player to the list
        $newcell = $('<td>').text(fourspaces + fourspaces + player.replace(/ /g, '\u00a0'));
        $newrow = $('<tr>').attr('class', 'partnerselected partnerchoice selected inmenu').attr('id', playerid.slice(1));
        $newrow.append($newcell);
        $('#opphead').before($newrow);
        if ($('#partnerhead').hasClass('open')) {$newrow.toggle();}
        $(playerid).click(function () {
            //var choiceid = $(this).attr('id')
            if (event.ctrlKey) {var ctrl=1;}
            else {var ctrl=0;}
            choiceClick(playerid.slice(1), 'partner', ctrl);
            });
        }
    else {$(playerid).addClass('selected').addClass('partnerselected');}
    //set time span to career
    if (make==1) {makeMatchTable({}, defSort);}
    }
    
function setOpponentFilter(player, make, defSort) {
    $('.oppchoice').removeClass('selected').removeClass('oppselected');
    var playerid = '#' + 'opp' + player.replace(/ /g, '');
    if ($(playerid).length == 0) { // new player to the list
        $newcell = $('<td>').text(fourspaces + fourspaces + player.replace(/ /g, '\u00a0'));
        $newrow = $('<tr>').attr('class', 'oppselected oppchoice selected inmenu').attr('id', playerid.slice(1));
        $newrow.append($newcell);
        $('#oppteamhead').before($newrow);
        if ($('#opphead').hasClass('open')) {$newrow.toggle();}
        $(playerid).click(function () {
            //var choiceid = $(this).attr('id')
            if (event.ctrlKey) {var ctrl=1;}
            else {var ctrl=0;}
            choiceClick(playerid.slice(1), 'opp', ctrl);
            });
        }
    else {$(playerid).addClass('selected').addClass('oppselected');}
    //set time span to career
    if (make==1) {makeMatchTable({}, defSort);}
    }
    
function setOppteamFilter(player, make, defSort) {
    $('.oppteamchoice').removeClass('selected').removeClass('oppteamselected');
    var playerid = '#' + 'oppteam' + player.replace(/ /g, '');
    if ($(playerid).length == 0) { // new player to the list
        var slash_replace = '/<br/>' + fourspaces + "&nbsp;" + fourspaces
        $newcell = $('<td>').html(fourspaces + fourspaces + player.replace(/ /g, '\u00a0').replace('/', slash_replace));
        $newrow = $('<tr>').attr('class', 'oppteamselected oppteamchoice selected inmenu').attr('id', playerid.slice(1));
        $newrow.append($newcell);
        $('#spanhead').before($newrow);
        if ($('#oppteamhead').hasClass('open')) {$newrow.toggle();}
        $(playerid).click(function () {
            //var choiceid = $(this).attr('id')
            if (event.ctrlKey) {var ctrl=1;}
            else {var ctrl=0;}
            choiceClick(playerid.slice(1), 'oppteam', ctrl);
            });
        }
    else {$(playerid).addClass('selected').addClass('oppteamselected');}
    //set time span to career
    if (make==1) {makeMatchTable({}, defSort);}
    }

function setNotPlayerFilter(player, make) {
    $('.notchoice').removeClass('selected').removeClass('notselected');
    var playerid = '#' + 'not' + player.replace(/ /g, '');
    if ($(playerid).length == 0) { // new player to the list
        $newcell = $('<td>').text(fourspaces + fourspaces + player.replace(/ /g, '\u00a0'));
        $newrow = $('<tr>').attr('class', 'notselected notchoice selected inmenu').attr('id', playerid.slice(1));
        $newrow.append($newcell);
        $('#spanhead').before($newrow);
        if ($('#nothead').hasClass('open')) {$newrow.toggle();}
        $(playerid).click(function () {
            //var choiceid = $(this).attr('id')
            if (event.ctrlKey) {var ctrl=1;}
            else {var ctrl=0;}
            choiceClick(playerid.slice(1), 'not', ctrl);
            });
        }
    else {$(playerid).addClass('selected').addClass('notselected');}
    //set time span to career
    if (make==1) {makeMatchTable({}, 0);}
    }

function fHeaderClick(choiceid) {
    var filt = choiceid.slice(0, -4);
    $("." + filt + "choice").toggle();
    if ($("#" + filt + "head").hasClass("closed")) {$("#" + filt + "head").removeClass("closed").addClass("open");}
    else {$("#" + filt + "head").removeClass("open").addClass("closed");}
    if ($("." + filt + "choice").is(':visible') == false && $("#" + filt + "def").hasClass("selected") == false) {
        // check if default is selected
        $("#" + filt + "head").addClass("selected");
        }
    else {$("#" + filt + "head").removeClass("selected");}
    }
    
var defParams = ''    
window.onpopstate = function (event) {
    if (event.state == null) {
        var prefs = 0;
        for (pf in prefilters) {prefs += 1;}
        if (prefs == 0) {newPrefilters(defParams);}
        }
    else if (event.state.new_text.indexOf('.cgi') != -1) {
        newPrefilters(defParams);
        }
    else {
        //alert('event state '+event.state.slug);
        newPrefilters(event.state.new_text);
        new_text = event.state.new_text;
        }
    };
    
var startdate = '', enddate = '';
var lowrank = '', highrank = '';
var statchoice = '', statoperatorchoice = '', statinput = '';
var statlist = ['Dom Ratio', 'Ace Perc', 'DF Perc', '1st In', '1st WPc', '2nd WPc', 'BP Svd Pc', 'BP Saved', 'BPC Faced',
                    'Time', 'TPW', 'RPW', 'vAce Pc', 'v1st WPc', 'v2nd WPc', 'BP Cnv Pc', 'BP Conv', 'BP Chncs', 'Tot Pts',
                    'Aces', 'DFs', 'Sv Pts', '1Sv Pts', '2Sv Pts', 'vAces'];
var statpercs = {'Ace Perc': 1, 'DF Perc': 1, '1st In': 1, '1st WPc': 1, '2nd WPc': 1, 'BP Svd Pc': 1, 
                     'TPW': 1, 'RPW': 1, 'vAce Pc': 1, 'v1st WPc': 1, 'v2nd WPc': 1, 'BP Cnv Pc': 1}

$(document).ready(function() { 

    if (typeof navbar !== 'undefined') {
        $("#navbar").html(navbar);
        } 

    //var pparams = getPermalinkParams();
    var thisurl = window.location.href
    if (thisurl.indexOf('&') == -1) {var pparams = '';}
    else {var pparams = thisurl.slice(thisurl.indexOf('&')).replace("&f=", "");}
    //else {var pparams = '';}
    if (typeof(history.replaceState) !== "undefined") {
        history.replaceState({
           //old_text: old_text.val(),
           new_text: pparams,
           slug: location.pathname.replace("/", "")
        }, null, null);
    }

    if (typeof photog == 'undefined' || photog == '') {
        insertBio();
        }
    else {
        insertPictureBio();
        }

    // add chart agg link to span id="shotsHere"
    if (typeof chartagg != 'undefined' && chartagg == 1) {
        $ca_span = $('<span />').addClass('tablink').addClass('tabview');
        $ca_span.css('background-color', '#e6EEEE').css('position', 'relative').css('top', '5px');
        var ca_link = 'https://www.tennisabstract.com/charting/' + fullname.replace(/ /g, '') + '.html'
        var ihtml = '&nbsp;<b><a href="' + ca_link + '" target="_blank" style="text-decoration:none">Shot-by-Shot Stats</a></b>&nbsp;';
        $ca_span.html(ihtml);
        $("#shotsHere").append($ca_span);
        // $('<br />').insertBefore($ca_span);
        }
   
    // if (typeof playernews === 'undefined') {var pnewsrows = '';} //|| playernews == ''
    // else if (playernews == '') {var pnewsrows = '';}
    // else {
    //     var pnewsrows = '';
    //     for (j=0; j<playernews.length; j++) {
    //         pnewsrows = pnewsrows + '<br/>' + playernews[j];
    //         }
    //     if (playernews.length > 0) {pnewsrows = pnewsrows + '<br/>&nbsp;';}
    //     }

    // if (typeof upcoming === 'undefined') {var upcomingrow = '';}
    // else if (upcoming == '') {var upcomingrow = '';}
    // else {
    //     if (month < 10) {var udate = day + ' ' + months['0'+month+''];}
    //     else {var udate = day + ' ' + months[month+''];}
    //     var upcomingrow = '<br/><b>' + udate + '</b> Upcoming Tournaments: ' + upcoming;} // add date dynamically
    //     //if (pnewsrows == '') {pnewsrows = '<br/>&nbsp;';}
    // if (pnewsrows == '' && upcomingrow == '') {} //  && upcomingrow == ''
    // else {
    //     var newsanalysis = '<span style="background-color:#e6EEEE;"><b>News and Analysis</b></span>'
    //     var yourlinkhere = '<i><a href="https://tennisabstract.com/main/sitesubmit.html">your link here?</a></i>'
    //     var pnews = '&nbsp;<br/>' + newsanalysis + fourspaces + yourlinkhere + upcomingrow + pnewsrows; // upcomingrow + pnewsrows;
    //     $("#playernews").html(pnews);
    //     }
    
    if (typeof careerjs_dubs === 'undefined') {  
        // in case doubles data didn't load / doesn't exist  
        $("#tabDubs").hide();
        $("#tabDubsSpacer").hide();
        careerjs_dubs = 0;
        view = "";
        }        
    else if (view == "") {  // check ranks / peak ranks to see if should default to dubs view
        if (typeof currentrank === 'undefined' || currentrank == "" || currentrank == "UNR") {currentrank = 3000;}
        if (typeof peakrank === 'undefined' || peakrank == "") {peakrank = 3000;}
        if (typeof current_dubs === 'undefined' || current_dubs == "" || current_dubs == "UNR") {current_dubs = 3000;}
        if (typeof peak_dubs === 'undefined' || peak_dubs == "") {peak_dubs = 3000;}
        if (currentrank <= 50 || peakrank <= 10) {}
        else if (currentrank > (current_dubs * 5)) {view = "doubles";}
        else if (currentrank == "" && peakrank > (peak_dubs * 5)) {view = "doubles";}
        else if (currentrank == 3000 && peakrank > (peak_dubs * 5)) {view = "doubles";}
        // can do this server-side...
        }
    
    if (view == "doubles") {
        makeSplitsTable(doubles=1);
        prefilters['overview'] = 1; // default, show ranks and full names
        filteropts['span'] = ychoices_dubs;
        filteropts['tourney'] = tchoices_dubs;
        filteropts['asrank'] = rchoices_dubs;     
        }
    else {
        makeSplitsTable()
        }
    
//    if (typeof photog != 'undefined' && photog != '') {
//        $(".moresplits").toggle();
//        }
        
    makeMenus();

    applyPrefilters();
    $("#tabHead").click(function () {
        // if switching from doubles, change menus:
        if (!$('#tabDubs').hasClass("tablink")) {
            makeSplitsTable();
            //if (typeof photog != 'undefined' && photog != '') {
            //    $(".moresplits").toggle();
            //    }
            filteropts['span'] = ychoices;
            filteropts['tourney'] = tchoices;
            filteropts['asrank'] = rchoices;
            makeMenus();
            }
        $(".tabview").addClass("tablink");
        $("#tabHead").removeClass("tablink");
        resetFilters();
        if ($("#spanhead").hasClass("closed")) {
            $("#spanhead").removeClass("closed").addClass("open") //.addClass("selected");
            $(".spanchoice").toggle();
            }
        $(".spanchoice").removeClass("selected").removeClass('spanselected');
        $("#spanCareerqq").addClass('selected').addClass('spanselected'); 
        // show all filters (heads + choices), then hide minimum
        $(".header").show();
        $("#h2hhead").hide();
        $(".h2hchoice").hide();
        $("#nothead").hide();
        $(".notchoice").hide();
        $("#minyearshead").hide(); 
        $(".minyearschoice").hide();
        $("#partnerhead").hide();
        $(".partnerchoice").hide();
        $("#opphead").hide();
        $(".oppchoice").hide();
        $("#oppteamhead").hide();
        $(".oppteamchoice").hide();
        $("#handshead").hide();
        $(".handschoice").hide(); 
        $("#prankhead").hide();
        $(".prankchoice").hide();
        $("#phandhead").hide();
        $(".phandchoice").hide();
        makeMatchTable({}, 1);
        })
    $("#tabEvents").click(function () {
        // if switching from doubles, change menus:
        if (!$('#tabDubs').hasClass("tablink")) {
            makeSplitsTable();
            filteropts['span'] = ychoices;
            filteropts['tourney'] = tchoices;
            filteropts['asrank'] = rchoices;
            makeMenus();
            }
        $(".tabview").addClass("tablink");
        $("#tabEvents").removeClass("tablink");
        resetFilters();
        if ($("#spanhead").hasClass("closed")) {
            $("#spanhead").removeClass("closed").addClass("open") //.addClass("selected");
            $(".spanchoice").toggle();
            }
        $(".spanchoice").removeClass("selected").removeClass('spanselected');
        $("#spanCareerqq").addClass('selected').addClass('spanselected'); 
        // show all filters (heads + choices), then hide minimum
        $(".header").show();
        $("#h2hhead").hide();
        $(".h2hchoice").hide();
        $("#nothead").hide();
        $(".notchoice").hide();
        $("#partnerhead").hide();
        $(".partnerchoice").hide();
        $("#opphead").hide();
        $(".oppchoice").hide();
        $("#oppteamhead").hide();
        $(".oppteamchoice").hide();
        $("#handshead").hide();
        $(".handschoice").hide(); 
        $("#prankhead").hide();
        $(".prankchoice").hide();
        $("#phandhead").hide();
        $(".phandchoice").hide();
        makeMatchTable({}, 1);
        })        
    $("#tabResults").click(function () {
        // if switching from doubles, change menus:
        if (!$('#tabDubs').hasClass("tablink")) {
            makeSplitsTable();
            prefilters['overall'] = 1; // default, show serving stats
            filteropts['span'] = ychoices;
            filteropts['tourney'] = tchoices;
            filteropts['asrank'] = rchoices;
            makeMenus();
            }
        $(".tabview").addClass("tablink");
        $("#tabResults").removeClass("tablink");
        resetFilters();
        if ($("#spanhead").hasClass("closed")) {
            $("#spanhead").removeClass("closed").addClass("open") //.addClass("selected");
            $(".spanchoice").toggle();
            }
        $(".spanchoice").removeClass("selected").removeClass('spanselected');
        $("#spandef").addClass('selected').addClass('spanselected'); 
        // show all filters (heads + choices), then hide h2h, not ... more?
        $(".header").show();
        $("#minimumhead").hide(); 
        $(".minimumchoice").hide(); 
        $("#minyearshead").hide(); 
        $(".minyearschoice").hide();  
        $("#partnerhead").hide();
        $(".partnerchoice").hide();
        $("#opphead").hide();
        $(".oppchoice").hide();
        $("#oppteamhead").hide();
        $(".oppteamchoice").hide();
        $("#handshead").hide();
        $(".handschoice").hide();   
        $("#prankhead").hide();
        $(".prankchoice").hide();
        $("#phandhead").hide();
        $(".phandchoice").hide();  
        makeMatchTable({}, 1);
        })          
    $("#tabDubs").click(function () {
        prefilters['overview'] = 1; // default, show ranks and full names
        // switching from singles, so change menus:
        makeSplitsTable(doubles=1);
        filteropts['span'] = ychoices_dubs;
        filteropts['tourney'] = tchoices_dubs;
        filteropts['asrank'] = rchoices_dubs;
        makeMenus();
        $(".tabview").addClass("tablink");
        $("#tabDubs").removeClass("tablink");
        $(".header").show();
        $("#h2hhead").hide();
        $("#nothead").hide(); 
        $("#minimumhead").hide();
        $(".minimumchoice").hide();
        $("#minyearshead").hide();
        $(".minyearschoice").hide();
        $("#crankhead").hide();
        $(".crankchoice").hide();
        $("#handhead").hide();
        $(".handchoice").hide();	
        $("#agehead").hide();
        $(".agechoice").hide();
        $("#heighthead").hide();
        $(".heightchoice").hide();
        $("#countryhead").hide();
        $(".countrychoice").hide();      
        makeMatchTable({}, 1);
        })       
    $(".stattab").click(function () {
        $(".stattab").addClass("likelink");
        $(this).removeClass("likelink");
        makeMatchTable({}, 1);
        })
    $(".revscore").click(function () {
        hidePermalink();
        if ($(".revscore").text() == 'Reverse Loss Scores') {
            $(".revscore").html('Standard Scores');
            makeMatchTable({}, 0);
            }
        else {
            $(".revscore").html('Reverse Loss Scores');
            makeMatchTable({}, 0);
            }
        })
    if (careerjs == 1 && keep_loading == 1) {
        if (view != "doubles" && 'span' in prefilters && (prefilters['span'] != '1' && prefilters['span'] != '2')) {matchmx = matchmx.concat(morematchmx);}  
        else {
            var careerurl = 'https://www.minorleaguesplits.com/tennisabstract/cgi-bin/jsmatches/' + nameparam + 'Career.js';
            $.getScript(careerurl, function() {
                matchmx = matchmx.concat(morematchmx);
                })
            }
        }
    if (careerjs_dubs == 1 && keep_loading == 1) {
        if (view == "doubles" && 'span' in prefilters && (prefilters['span'] != '1' && prefilters['span'] != '2')) {matchmx_dubs = matchmx_dubs.concat(morematchmx_dubs);}  
        else {
            var careerurl = 'https://www.minorleaguesplits.com/tennisabstract/cgi-bin/jsdoubles/' + nameparam + 'Career.js';
            $.getScript(careerurl, function() {
                matchmx_dubs = matchmx_dubs.concat(morematchmx_dubs);
                })
            }
        }
    makeMatchTable({}, 0);
    $.getScript("https://www.tennisabstract.com/jquery.ui.core.js", function() {});
    $.getScript("https://www.tennisabstract.com/jquery.ui.position.js", function() {});
    $.getScript("https://www.tennisabstract.com/jquery.ui.widget.js", function() {});
    $.getScript("https://www.tennisabstract.com/mwplayerlist.js", function() {}); 
    $.getScript("https://www.tennisabstract.com/jquery.ui.autocomplete.js", function() {});    
    $('#playersearch').append($('<input>').attr('id', 'tags'));

    var searchbox = "Player Search";		
    $("#tags").val(searchbox);
    $("#tags").focus(function() {
        if ($(this).val() == searchbox) {$(this).val("");}
        $( "#tags" ).autocomplete({
                source: playerlist,
                minLength: 2,
                select: function(e, ui) {
                    var playerselect = ui.item.value;
                    var player = playerselect.slice(4);
                    var mw = playerselect.slice(1,2);
                    if (mw == 'M') {
                        var playerurl = 'https://www.tennisabstract.com/cgi-bin/player.cgi?p=' + player.replace(/ /g, '');
                        }
                    else {
                        var playerurl = 'https://www.tennisabstract.com/cgi-bin/wplayer.cgi?p=' + player.replace(/ /g, '');
                        }                        
                    window.open(playerurl, "_self");
                    }
            });
        }); 
    })

</script>
</head>
<body>

<div id="header">

<div id="navbar">
</div>

<table width=1240px>
<tr><td>&nbsp;</td><td>&nbsp;</td>
</tr>

<tr>
<td align="left" style="vertical-align:top"><span id="bio">&nbsp;</span>

<span id="tabResults" style="background-color:#e6EEEE;" class="tabview">&nbsp;<b>Singles Results</b>&nbsp;</span>&nbsp;
<span id="tabHead" style="background-color:#e6EEEE;" class="tablink tabview">&nbsp;<b>Head-to-Heads</b>&nbsp;</span>&nbsp;
<span id="tabEvents" style="background-color:#e6EEEE;" class="tablink tabview">&nbsp;<b>Event Records</b>&nbsp;</span>
<br/><span id="tabDubs" style="background-color:#e6EEEE; position:relative; top:5px" class="tablink tabview">&nbsp;<b>Doubles Results</b>&nbsp;</span><span id="tabDubsSpacer">&nbsp;&nbsp;</span>
<span id="shotsHere">
</span>
</td>
<td id="wonloss" align="right" style="vertical-align:top">&nbsp;</td>
</tr>
<tr>
<td id="tabmenu" align="left" style="vertical-align:top">
&nbsp;
</td>
<td>&nbsp;</td>
</tr>
<tr>
<td colspan=2 id="playernews">&nbsp;</td>
</tr>
</tr></table>
</div>

<div id="main">

<table width="1240px" id="maintable">
<tr id="tabletoggles">
<td>&nbsp;</td>
<td id="tablelabel">&nbsp;</td>
<td id="abovestats" class="abovestats" align="right">
&nbsp;&nbsp;&nbsp;<span class="revscore likelink"></span>
&nbsp;&nbsp;&nbsp;<b>Stats:</b>&nbsp;
<span class="statsa stattab">Overview</span><span class="statspacer">&nbsp;|&nbsp;</span><span class="statso stattab">Serve</span>&nbsp;|&nbsp;<span class="statsr stattab likelink">Return</span>&nbsp;|&nbsp;<span class="statsw stattab likelink">Raw</span>
</td></tr>
<tr>
<td id="footer" class="footer">&nbsp;</td>
<td colspan="2" id="stats" class="stats"><table id="matches"></table></td>
</tr>
<tr>
<td id="belowmenus">&nbsp;<br/>&nbsp;<br/>&nbsp;<br/>&nbsp;<br/>&nbsp;</td>
<td colspan="2" id="belowmatches">&nbsp;</td>
</tr>
</table></div>
</div>

</body>

