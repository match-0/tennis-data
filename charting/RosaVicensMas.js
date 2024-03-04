var hand = 'R';

<html><head>
<title>Tennis Abstract: Rosa Vicens Mas Match Results, Splits, and Analysis</title>
<link rel="stylesheet" href="https://www.tennisabstract.com/blue/style.css" type="text/css">
<script type="text/javascript" src="https://www.tennisabstract.com/jquery-1.7.1-min.js"></script>
<script type="text/javascript" src="https://www.tennisabstract.com/jquery.tablesorter.js"></script>
<script type="text/javascript" src="https://www.tennisabstract.com/navbar.js"></script>

<script type="text/javascript" src="https://www.minorleaguesplits.com/tennisabstract/cgi-bin/frags/RosaVicensMas.js"></script>
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
var nameparam = 'RosaVicensMas';





var fullname = 'Rosa Vicens Mas';
var lastname = 'Vicens Mas';
var currentrank = 301;
var peakrank = 175;
var peakfirst = 20230417;
var peaklast = 20230417;
var dob = 20000625;
var ht = '';
var hand = 'U';
var backhand = '';
var country = 'ESP';
var shortlist = 0;
var careerjs = 0;
var active = 1;
var lastdate = 0;
var twitter = '';
var current_dubs = "UNR";
var peak_dubs = "UNR";
var peakfirst_dubs = "";
var liverank = "";
var chartagg = 0;
var photog = '';
var photog_credit = '';
var photog_link = '';
var itf_id = '';
var atp_id = '';
var dc_id = '';
var wiki_id = '';
 

var fourspaces = "\u00a0\u00a0\u00a0\u00a0";
                  
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

var bhdict = {'1': 'one', '2': 'two'};

function insertPictureBio() {
    var plname = '<span style="font-size:18px"><b>' + fullname + ' [' + country + ']</b></span>';
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
        
    // if (peak_dubs == "" || peakfirst_dubs == "") {
    //     var drank = '&nbsp;';
    //     }
    // else if (typeof current_dubs === 'undefined' || current_dubs == "" || current_dubs == "UNR") {
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

    biotable = biotable + '<tr><td>&nbsp;</td></tr>';
    biotable = biotable + '<tr><td>' + '<i>Photo: <a target="_blank" href="' + photog_link + '">' + photog_credit + '</a></i></td></tr>';

    $("#bio").html(biotable + '</table></td><td>&nbsp;&nbsp;</td></tr></table></p>');
    }

function insertBio() {
    var plname = '<span style="font-size:18px"><b>' + fullname + ' [' + country + ']</b></span>';
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
    else {var profile_row = '<tr><td>&nbsp;</td><td>&nbsp;</td></tr>';}      

    $("#bio").html(biotable + rowtwo + rowthree + profile_row + '</table></p>');
    }    

$(document).ready(function() { 

    //$("#navbar").html(navbar);
    
    if (typeof navbar !== 'undefined') {
        $("#navbar").html(navbar);
        } 

    if (typeof photog == 'undefined' || photog == '') {
        insertBio();
        }
    else {
        insertPictureBio();
        }
        
    $("#main").html(player_frag)
    $("#menu").html(frag_menu)
    
    $("#recent-results").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#recent-results").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#recent-events").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#recent-events").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );                                   

    $("#recent-finals").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#recent-finals").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#tour-years").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#tour-years").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#chall-years").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#chall-years").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#career-splits").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#career-splits").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#last52-splits").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#last52-splits").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#news-analysis").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#news-analysis").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#mcp-serve").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#mcp-serve").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#mcp-return").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#mcp-return").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#mcp-rally").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#mcp-rally").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );

    $("#mcp-tactics").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#mcp-tactics").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#head-to-heads").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#head-to-heads").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#tour-results").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#tour-results").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#match-results").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#match-results").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#career-splits-chall").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#career-splits-chall").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#last52-splits-chall").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#last52-splits-chall").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#year-end-rankings").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#year-end-rankings").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#titles-finals").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#titles-finals").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#doubles").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#doubles").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false}
                                    }}
                                   );
    $("#mixed-doubles").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#mixed-doubles").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false}
                                    }}
                                   );
                                   
    $("#pbp-points").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#pbp-points").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#pbp-games").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#pbp-games").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#pbp-stats").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#pbp-stats").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#serve-speed").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#serve-speed").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
    $("#winners-errors").attr('border', 0).attr('cellspacing', 0).attr('cellpadding', 4);
    $("#winners-errors").tablesorter( {headers: {0: {sorter:false}, 1: {sorter:false}, 2: {sorter:false}, 3: {sorter:false},
                                                 4: {sorter:false}, 5: {sorter:false}, 6: {sorter:false}, 7: {sorter:false},
                                                8: {sorter:false}, 9: {sorter:false}, 10: {sorter:false}, 11: {sorter:false},
                                                12: {sorter:false}, 13: {sorter:false}, 14: {sorter:false}, 15: {sorter:false},
                                                16: {sorter:false}, 17: {sorter:false}, 18: {sorter:false}, 19: {sorter:false},
                                                20: {sorter:false}, 21: {sorter:false}, 22: {sorter:false}, 23: {sorter:false},
                                                24: {sorter:false}, 25: {sorter:false}, 26: {sorter:false}, 27: {sorter:false}
                                    }}
                                   );
                                   
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
<style type="text/css">
u {    
    border-bottom: 1px dotted #000;
    text-decoration: none;
}

</style>
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
</td>
<td id="menu" align="right" style="vertical-align:top">&nbsp;</td>
</tr>
</tr></table>
</div>

<div id="main">

</div>
</div>

</body>

