import pandas as pd
import numpy as np
from datetime import date, datetime, timedelta
import math
import sys
import re

player1_name = sys.argv[1]   # "Novak Djokovic"
player2_name = sys.argv[2]   #"All Right-Hand"   #"All Right-Hand"
target_match_date = sys.argv[3] #"2022-11-08"  #yyyy-MM-dd
#p1_winning_chance = 0.627  # this should be calculated
report_page1_name=re.sub('[^0-9A-Za-z]', '-', 'generated-report-page1-%s-%s-%s'%(player1_name, player2_name, target_match_date))+'.html'
report_page2_name=re.sub('[^0-9A-Za-z]', '-', 'generated-report-page2-%s-%s-%s'%(player1_name, player2_name, target_match_date))+'.html'

def add_years(d, years):
    """Return a date that's `years` years after the date (or datetime)
    object `d`. Return the same calendar date (month and day) in the
    destination year, if it exists, otherwise use the following day
    (thus changing February 29 to March 1).

    """
    try:
        return d.replace(year = d.year + years)
    except ValueError:
        return d + (date(d.year + years, 1, 1) - date(d.year, 1, 1))

def LargestRemainderRound(inputs, expected_sum=100):
    # https://stackoverflow.com/questions/5227215/how-to-deal-with-the-sum-of-rounded-percentage-not-being-100
    sum = 0
    for e in inputs:
        sum += e
    outputs, remainders = [0] * len(inputs), [0.0] * len(inputs)
    outputs_sum = 0
    for i in range(len(inputs)):
        outputs[i] = math.floor(inputs[i] * expected_sum / sum)
        outputs_sum += outputs[i]
        remainders[i] = inputs[i] * expected_sum / sum - outputs[i]
    remainder_int = expected_sum - outputs_sum
    indices = np.argsort(remainders)[::-1]
    for i in range(remainder_int):
        outputs[indices[i]] += 1
    return  outputs

def region_size(percentage):
    if percentage>60:
        return "xx-big"
    elif percentage>50:
        return "x-big"
    elif percentage>40:
        return "big"
    elif percentage>30:
        return "mid"
    elif percentage>20:
        return "small"
    elif percentage>10:
        return "x-small"
    else:
        return "xx-small"

df_all = pd.read_csv('tennisabstract-v2-combined.csv', header=None)
df_all[47] = df_all[45].str[28:]  # remove the folder name in the match_id
match_all = pd.read_csv('tennisabstract-v2-match-list.csv')

# The first step is to find a list of matches, which provides source data for the report
start_date = (add_years(datetime.strptime(target_match_date, '%Y-%m-%d').date(), -2)).strftime('%Y%m%d')
target_match_date = datetime.strptime(target_match_date, '%Y-%m-%d').strftime('%Y%m%d')
if player1_name != "All Right-Hand" and player1_name != "All Left-Hand" \
and player2_name != "All Right-Hand" and player2_name != "All Left-Hand":
    hist_matches = match_all[(match_all.match_id < target_match_date)
                            &(match_all.match_id > start_date)
                            &( ( (match_all.player1 == player1_name) & (match_all.player2 == player2_name) 
                               | (match_all.player2 == player1_name) & (match_all.player1 == player2_name) ) )]
elif player2_name == "All Right-Hand":
    hist_matches = match_all[(match_all.match_id < target_match_date)
                            &(match_all.match_id > start_date)
                            &( ( (match_all.player1 == player1_name) & (match_all.player2hand == "RH") 
                               | (match_all.player2 == player1_name) & (match_all.player1hand == "RH") ) )]
elif player2_name == "All Left-Hand":
    hist_matches = match_all[(match_all.match_id < target_match_date)
                            &(match_all.match_id > start_date)
                            &( ( (match_all.player1 == player1_name) & (match_all.player2hand == "LH") 
                               | (match_all.player2 == player1_name) & (match_all.player1hand == "LH") ) )]
# elif player1_name == "All Right-Hand":
#     hist_matches = match_all[(match_all.match_id < target_match_date)
#                             &(match_all.match_id > start_date)
#                             &( ( (match_all.player1 == player2_name) & (match_all.player2hand == "RH") 
#                                | (match_all.player2 == player2_name) & (match_all.player1hand == "RH") ) )]
# elif player1_name == "All Left-Hand":
#     hist_matches = match_all[(match_all.match_id < target_match_date)
#                             &(match_all.match_id > start_date)
#                             &( ( (match_all.player1 == player2_name) & (match_all.player2hand == "LH") 
#                                | (match_all.player2 == player2_name) & (match_all.player1hand == "LH") ) )]
    
df_source = df_all[df_all[47].isin(hist_matches.match_id)]

# player1 deuce court 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[13]==1) & (df_source[12]==1)][15].value_counts()
p1_de_serve_1st_t = temp_data[6] if 6 in temp_data.index else 0
p1_de_serve_1st_wide = temp_data[4] if 4 in temp_data.index else 0
p1_de_serve_1st_body = temp_data[5] if 5 in temp_data.index else 0
print("player1 deuce court 1st serve to (t, body, wide):", p1_de_serve_1st_t, p1_de_serve_1st_body, p1_de_serve_1st_wide)

# player1 AD court 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[13]==3) & (df_source[12]==1)][15].value_counts()
p1_ad_serve_1st_t = temp_data[6] if 6 in temp_data.index else 0
p1_ad_serve_1st_wide = temp_data[4] if 4 in temp_data.index else 0
p1_ad_serve_1st_body = temp_data[5] if 5 in temp_data.index else 0
print("player1 ad court 1st serve to (t, body, wide):", p1_ad_serve_1st_t, p1_ad_serve_1st_body, p1_ad_serve_1st_wide)

# player1 deuce court 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[13]==1) & (df_source[12]==2)][15].value_counts()
p1_de_serve_2nd_t = temp_data[6] if 6 in temp_data.index else 0
p1_de_serve_2nd_wide = temp_data[4] if 4 in temp_data.index else 0
p1_de_serve_2nd_body = temp_data[5] if 5 in temp_data.index else 0
print("player1 deuce court 2nd serve to (t, body, wide):", p1_de_serve_2nd_t, p1_de_serve_2nd_body, p1_de_serve_2nd_wide)

# player1 AD court 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[13]==3) & (df_source[12]==2)][15].value_counts()
p1_ad_serve_2nd_t = temp_data[6] if 6 in temp_data.index else 0
p1_ad_serve_2nd_wide = temp_data[4] if 4 in temp_data.index else 0
p1_ad_serve_2nd_body = temp_data[5] if 5 in temp_data.index else 0
print("player1 ad court 2nd serve to (t, body, wide):", p1_ad_serve_2nd_t, p1_ad_serve_2nd_body, p1_ad_serve_2nd_wide)

# player1 15-40 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12]==1)
                     &(df_source[4]==1) & (df_source[5]==3)][15].value_counts()
p1_serve_1st_t_15_40 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_1st_wide_15_40 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_1st_body_15_40 = temp_data[5] if 5 in temp_data.index else 0
print("player1 15-40 1st serve to (t, body, wide):", p1_serve_1st_t_15_40, p1_serve_1st_body_15_40, p1_serve_1st_wide_15_40)

# player1 15-40 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12]==2)
                     &(df_source[4]==1) & (df_source[5]==3)][15].value_counts()
p1_serve_2nd_t_15_40 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_2nd_wide_15_40 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_2nd_body_15_40 = temp_data[5] if 5 in temp_data.index else 0
print("player1 15-40 2nd serve to (t, body, wide):", p1_serve_2nd_t_15_40, p1_serve_2nd_body_15_40, p1_serve_2nd_wide_15_40)

# player1 30-30 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12]==1)
                     &(df_source[4]==2) & (df_source[5]==2)][15].value_counts()
p1_serve_1st_t_30_30 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_1st_wide_30_30 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_1st_body_30_30 = temp_data[5] if 5 in temp_data.index else 0
print("player1 30-30 1st serve to (t, body, wide):", p1_serve_1st_t_30_30, p1_serve_1st_body_30_30, p1_serve_1st_wide_30_30)

# player1 30-30 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12]==2)
                     &(df_source[4]==2) & (df_source[5]==2)][15].value_counts()
p1_serve_2nd_t_30_30 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_2nd_wide_30_30 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_2nd_body_30_30 = temp_data[5] if 5 in temp_data.index else 0
print("player1 30-30 2nd serve to (t, body, wide):", p1_serve_2nd_t_30_30, p1_serve_2nd_body_30_30, p1_serve_2nd_wide_30_30)

# player1 30-40 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12]==1)
                     &(df_source[4]==2) & (df_source[5]==3)][15].value_counts()
p1_serve_1st_t_30_40 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_1st_wide_30_40 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_1st_body_30_40 = temp_data[5] if 5 in temp_data.index else 0
print("player1 30-40 1st serve to (t, body, wide):", p1_serve_1st_t_30_40, p1_serve_1st_body_30_40, p1_serve_1st_wide_30_40)

# player1 30-40 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12]==2)
                     &(df_source[4]==2) & (df_source[5]==3)][15].value_counts()
p1_serve_2nd_t_30_40 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_2nd_wide_30_40 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_2nd_body_30_40 = temp_data[5] if 5 in temp_data.index else 0
print("player1 30-40 2nd serve to (t, body, wide):", p1_serve_2nd_t_30_40, p1_serve_2nd_body_30_40, p1_serve_2nd_wide_30_40)

# player1 40-AD 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12]==1)
                     &(df_source[4]==3) & (df_source[5]==4)][15].value_counts()
p1_serve_1st_t_40_ad = temp_data[6] if 6 in temp_data.index else 0
p1_serve_1st_wide_40_ad = temp_data[4] if 4 in temp_data.index else 0
p1_serve_1st_body_40_ad = temp_data[5] if 5 in temp_data.index else 0
print("player1 40-AD 1st serve to (t, body, wide):", p1_serve_1st_t_40_ad, p1_serve_1st_body_40_ad, p1_serve_1st_wide_40_ad)

# player1 40-AD 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12]==2)
                     &(df_source[4]==3) & (df_source[5]==4)][15].value_counts()
p1_serve_2nd_t_40_ad = temp_data[6] if 6 in temp_data.index else 0
p1_serve_2nd_wide_40_ad = temp_data[4] if 4 in temp_data.index else 0
p1_serve_2nd_body_40_ad = temp_data[5] if 5 in temp_data.index else 0
print("player1 40-AD 2nd serve to (t, body, wide):", p1_serve_2nd_t_40_ad, p1_serve_2nd_body_40_ad, p1_serve_2nd_wide_40_ad)

p1_serve_1st_wide_break_point = p1_serve_1st_wide_30_40 + p1_serve_1st_wide_40_ad
p1_serve_1st_body_break_point = p1_serve_1st_body_30_40 + p1_serve_1st_body_40_ad
p1_serve_1st_t_break_point = p1_serve_1st_t_30_40 + p1_serve_1st_t_40_ad
print("player1 break point 1st serve to (t, body, wide):", p1_serve_1st_t_break_point, p1_serve_1st_body_break_point, p1_serve_1st_wide_break_point)

p1_serve_2nd_wide_break_point = p1_serve_2nd_wide_30_40 + p1_serve_2nd_wide_40_ad
p1_serve_2nd_body_break_point = p1_serve_2nd_body_30_40 + p1_serve_2nd_body_40_ad
p1_serve_2nd_t_break_point = p1_serve_2nd_t_30_40 + p1_serve_2nd_t_40_ad
print("player1 break point 2nd serve to (t, body, wide):", p1_serve_2nd_t_break_point, p1_serve_2nd_body_break_point, p1_serve_2nd_wide_break_point)

df_return = df_source[(df_source[0] == player1_name) & (df_source[12]==3)]

# player1 forehand return direction count
if df_return.iloc[0][2] == "RH": # player1 is RH
    temp_data = df_return[((df_return[26]==4) & (df_return[24]==1))
                         |((df_return[26]==6) & (df_return[24]==3))][16].value_counts()
    pl_return_fh_left = temp_data[1] if 1 in temp_data.index else 0
    p1_return_fh_mid = temp_data[2] if 2 in temp_data.index else 0
    p1_return_fh_right = temp_data[3] if 3 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26]==6) & (df_return[24]==1))
                         |((df_return[26]==4) & (df_return[24]==3))][16].value_counts()
    pl_return_fh_left = temp_data[1] if 1 in temp_data.index else 0
    p1_return_fh_mid = temp_data[2] if 2 in temp_data.index else 0
    p1_return_fh_right = temp_data[3] if 3 in temp_data.index else 0
print("player1 forehand return to (left, middle, right):", pl_return_fh_left, p1_return_fh_mid, p1_return_fh_right)

# player1 backhand return direction count
if df_return.iloc[0][2] == "RH": # player1 is RH
    temp_data = df_return[((df_return[26]==6) & (df_return[24]==1))
                         |((df_return[26]==4) & (df_return[24]==3))][16].value_counts()
    pl_return_bh_left = temp_data[1] if 1 in temp_data.index else 0
    p1_return_bh_mid = temp_data[2] if 2 in temp_data.index else 0
    p1_return_bh_right = temp_data[3] if 3 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26]==4) & (df_return[24]==1))
                         |((df_return[26]==6) & (df_return[24]==3))][16].value_counts()
    pl_return_bh_left = temp_data[1] if 1 in temp_data.index else 0
    p1_return_bh_mid = temp_data[2] if 2 in temp_data.index else 0
    p1_return_bh_right = temp_data[3] if 3 in temp_data.index else 0
print("player1 backhand return to (left, middle, right):", pl_return_bh_left, p1_return_bh_mid, p1_return_bh_right)

# player1 body return direction count
temp_data = df_return[(df_return[26]==5)][16].value_counts()
pl_return_body_left = temp_data[1] if 1 in temp_data.index else 0
p1_return_body_mid = temp_data[2] if 2 in temp_data.index else 0
p1_return_body_right = temp_data[3] if 3 in temp_data.index else 0
print("player1 body return to (left, middle, right):", pl_return_body_left, p1_return_body_mid, p1_return_body_right)

# player1 forehand return depth count
if df_return.iloc[0][2] == "RH": # player1 is RH
    temp_data = df_return[((df_return[26]==4) & (df_return[24]==1))
                         |((df_return[26]==6) & (df_return[24]==3))][17].value_counts()
    pl_return_fh_shallow = temp_data[1] if 1 in temp_data.index else 0
    p1_return_fh_deep = temp_data[2] if 2 in temp_data.index else 0
    p1_return_fh_deep += temp_data[3] if 3 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26]==6) & (df_return[24]==1))
                         |((df_return[26]==4) & (df_return[24]==3))][17].value_counts()
    pl_return_fh_shallow = temp_data[1] if 1 in temp_data.index else 0
    p1_return_fh_deep = temp_data[2] if 2 in temp_data.index else 0
    p1_return_fh_deep += temp_data[3] if 3 in temp_data.index else 0
print("player1 forehand return to (shallow, deep):", pl_return_fh_shallow, p1_return_fh_deep)

# player1 backhand return depth count
if df_return.iloc[0][2] == "RH": # player1 is RH
    temp_data = df_return[((df_return[26]==6) & (df_return[24]==1))
                         |((df_return[26]==4) & (df_return[24]==3))][17].value_counts()
    pl_return_bh_shallow = temp_data[1] if 1 in temp_data.index else 0
    p1_return_bh_deep = temp_data[2] if 2 in temp_data.index else 0
    p1_return_bh_deep += temp_data[3] if 3 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26]==4) & (df_return[24]==1))
                         |((df_return[26]==6) & (df_return[24]==3))][17].value_counts()
    pl_return_bh_shallow = temp_data[1] if 1 in temp_data.index else 0
    p1_return_bh_deep = temp_data[2] if 2 in temp_data.index else 0
    p1_return_bh_deep += temp_data[3] if 3 in temp_data.index else 0
print("player1 backhand return to (shallow, deep):", pl_return_bh_shallow, p1_return_bh_deep)

# player1 body return depth count
temp_data = df_return[(df_return[26]==5)][17].value_counts()
pl_return_body_shallow = temp_data[1] if 1 in temp_data.index else 0
p1_return_body_deep = temp_data[2] if 2 in temp_data.index else 0
p1_return_body_deep += temp_data[3] if 3 in temp_data.index else 0
print("player1 body return to (shallow, deep):", pl_return_body_shallow, p1_return_body_deep)

# player1 forehand return error count
if df_return.iloc[0][2] == "RH": # player1 is RH
    temp_data = df_return[((df_return[26]==4) & (df_return[24]==1))
                         |((df_return[26]==6) & (df_return[24]==3))][21].value_counts()
    pl_return_fh_error = temp_data[3] if 3 in temp_data.index else 0
    pl_return_fh_error += temp_data[4] if 4 in temp_data.index else 0
    temp_data = df_source[(df_source[0] != player1_name) 
                        & ((df_source[12]==1) | (df_source[12]==2))
                        & ( ((df_source[15]==4) & (df_source[13]==1))
                          | ((df_source[15]==6) & (df_source[13]==3)) )][21].value_counts()
    oppo_serve_ace_fh = temp_data[1] if 1 in temp_data.index else 0
    oppo_serve_winner_fh = temp_data[6] if 6 in temp_data.index else 0
    oppo_serve_in_fh = temp_data[7] if 7 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26]==6) & (df_return[24]==1))
                         |((df_return[26]==4) & (df_return[24]==3))][21].value_counts()
    pl_return_fh_error = temp_data[3] if 3 in temp_data.index else 0
    pl_return_fh_error += temp_data[4] if 4 in temp_data.index else 0
    temp_data = df_source[(df_source[0] != player1_name) 
                        & ((df_source[12]==1) | (df_source[12]==2))
                        & ( ((df_source[15]==6) & (df_source[13]==1))
                          | ((df_source[15]==4) & (df_source[13]==3)) )][21].value_counts()
    oppo_serve_ace_fh = temp_data[1] if 1 in temp_data.index else 0
    oppo_serve_winner_fh = temp_data[6] if 6 in temp_data.index else 0
    oppo_serve_in_fh = temp_data[7] if 7 in temp_data.index else 0
print("player1 forehand return (error, server ace, total):", 
      pl_return_fh_error+oppo_serve_winner_fh, oppo_serve_ace_fh, oppo_serve_ace_fh+oppo_serve_winner_fh+oppo_serve_in_fh)

# player1 backhand return error count
if df_return.iloc[0][2] == "RH": # player1 is RH
    temp_data = df_return[((df_return[26]==6) & (df_return[24]==1))
                         |((df_return[26]==4) & (df_return[24]==3))][21].value_counts()
    pl_return_bh_error = temp_data[3] if 3 in temp_data.index else 0
    pl_return_bh_error += temp_data[4] if 4 in temp_data.index else 0
    temp_data = df_source[(df_source[0] != player1_name) 
                        & ((df_source[12]==1) | (df_source[12]==2))
                        & ( ((df_source[15]==6) & (df_source[13]==1))
                          | ((df_source[15]==4) & (df_source[13]==3)) )][21].value_counts()
    oppo_serve_ace_bh = temp_data[1] if 1 in temp_data.index else 0
    oppo_serve_winner_bh = temp_data[6] if 6 in temp_data.index else 0
    oppo_serve_in_bh = temp_data[7] if 7 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26]==4) & (df_return[24]==1))
                         |((df_return[26]==6) & (df_return[24]==3))][21].value_counts()
    pl_return_bh_error = temp_data[3] if 3 in temp_data.index else 0
    pl_return_bh_error += temp_data[4] if 4 in temp_data.index else 0
    temp_data = df_source[(df_source[0] != player1_name) 
                        & ((df_source[12]==1) | (df_source[12]==2))
                        & ( ((df_source[15]==4) & (df_source[13]==1))
                          | ((df_source[15]==6) & (df_source[13]==3)) )][21].value_counts()
    oppo_serve_ace_bh = temp_data[1] if 1 in temp_data.index else 0
    oppo_serve_winner_bh = temp_data[6] if 6 in temp_data.index else 0
    oppo_serve_in_bh = temp_data[7] if 7 in temp_data.index else 0
print("player1 backhand return (error, server ace, total):", 
      pl_return_bh_error+oppo_serve_winner_bh, oppo_serve_ace_bh, oppo_serve_ace_bh+oppo_serve_winner_bh+oppo_serve_in_bh)

# player1 body return error count
temp_data = df_return[(df_return[26]==5)][21].value_counts()
pl_return_body_error = temp_data[3] if 3 in temp_data.index else 0
pl_return_body_error += temp_data[4] if 4 in temp_data.index else 0
temp_data = df_source[(df_source[0] != player1_name) 
                        & ((df_source[12]==1) | (df_source[12]==2))
                        & (df_source[15]==5) ][21].value_counts()
oppo_serve_ace_body = temp_data[1] if 1 in temp_data.index else 0
oppo_serve_winner_body = temp_data[6] if 6 in temp_data.index else 0
oppo_serve_in_body = temp_data[7] if 7 in temp_data.index else 0
print("player1 body return (error, server ace, total):", 
      pl_return_body_error+oppo_serve_winner_body, oppo_serve_ace_body, oppo_serve_ace_body+oppo_serve_winner_body+oppo_serve_in_body)

template = open('tennis-data-report-template-v2-single-page.html', 'r')
all_lines = template.readlines()
out_lines = []
for line in all_lines:
    x = line.replace('"p1_name"', player1_name)
    x = x.replace('"p1_short_name"', player1_name.split(' ')[1][:15])
    x = x.replace('"p2_name"', player2_name)
    x = x.replace('"p2_short_name"', player2_name.split(' ')[1][:15])
    x = x.replace('"start_date"', datetime.strptime(start_date, '%Y%m%d').strftime('%Y-%m-%d'))
    x = x.replace('"target_match_date"', datetime.strptime(target_match_date, '%Y%m%d').strftime('%Y-%m-%d'))
    x = x.replace('"page1.html"', report_page1_name)
    x = x.replace('"page2.html"', report_page2_name)
#    x = x.replace('"p1_winning_chance"', '{:.1%}'.format(p1_winning_chance))
    
    if (p1_ad_serve_1st_wide+p1_ad_serve_1st_body+p1_ad_serve_1st_t)>0:
        rounded = LargestRemainderRound([p1_ad_serve_1st_wide, p1_ad_serve_1st_body, p1_ad_serve_1st_t])
        x = x.replace('"p1_ad_serve_1st_wide"', '%d%%'%rounded[0])
        x = x.replace('"p1_ad_serve_1st_wide_size"', region_size(rounded[0]))
        x = x.replace('"p1_ad_serve_1st_body"', '%d%%'%rounded[1])
        x = x.replace('"p1_ad_serve_1st_body_size"', region_size(rounded[1]))
        x = x.replace('"p1_ad_serve_1st_t"', '%d%%'%rounded[2])
        x = x.replace('"p1_ad_serve_1st_t_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_ad_serve_1st_wide"', '')
        x = x.replace('"p1_ad_serve_1st_wide_size"', "mid")
        x = x.replace('"p1_ad_serve_1st_body"', '')
        x = x.replace('"p1_ad_serve_1st_body_size"', "mid")
        x = x.replace('"p1_ad_serve_1st_t"', '')
        x = x.replace('"p1_ad_serve_1st_t_size"', "mid")

    if (p1_de_serve_1st_t+p1_de_serve_1st_body+p1_de_serve_1st_wide)>0:
        rounded = LargestRemainderRound([p1_de_serve_1st_t, p1_de_serve_1st_body, p1_de_serve_1st_wide])
        x = x.replace('"p1_de_serve_1st_t"', '%d%%'%rounded[0])
        x = x.replace('"p1_de_serve_1st_t_size"', region_size(rounded[0]))
        x = x.replace('"p1_de_serve_1st_body"', '%d%%'%rounded[1])
        x = x.replace('"p1_de_serve_1st_body_size"', region_size(rounded[1]))
        x = x.replace('"p1_de_serve_1st_wide"', '%d%%'%rounded[2])
        x = x.replace('"p1_de_serve_1st_wide_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_de_serve_1st_t"', '')
        x = x.replace('"p1_de_serve_1st_t_size"', "mid")
        x = x.replace('"p1_de_serve_1st_body"', '')
        x = x.replace('"p1_de_serve_1st_body_size"', "mid")
        x = x.replace('"p1_de_serve_1st_wide"', '')
        x = x.replace('"p1_de_serve_1st_wide_size"', "mid")
    
    if (p1_ad_serve_2nd_wide+p1_ad_serve_2nd_body+p1_ad_serve_2nd_t)>0:
        rounded = LargestRemainderRound([p1_ad_serve_2nd_wide, p1_ad_serve_2nd_body, p1_ad_serve_2nd_t])
        x = x.replace('"p1_ad_serve_2nd_wide"', '%d%%'%rounded[0])
        x = x.replace('"p1_ad_serve_2nd_wide_size"', region_size(rounded[0]))
        x = x.replace('"p1_ad_serve_2nd_body"', '%d%%'%rounded[1])
        x = x.replace('"p1_ad_serve_2nd_body_size"', region_size(rounded[1]))
        x = x.replace('"p1_ad_serve_2nd_t"', '%d%%'%rounded[2])
        x = x.replace('"p1_ad_serve_2nd_t_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_ad_serve_2nd_wide"', '')
        x = x.replace('"p1_ad_serve_2nd_wide_size"', "mid")
        x = x.replace('"p1_ad_serve_2nd_body"', '')
        x = x.replace('"p1_ad_serve_2nd_body_size"', "mid")
        x = x.replace('"p1_ad_serve_2nd_t"', '')
        x = x.replace('"p1_ad_serve_2nd_t_size"', "mid")
    
    if (p1_de_serve_2nd_t+p1_de_serve_2nd_body+p1_de_serve_2nd_wide)>0:
        rounded = LargestRemainderRound([p1_de_serve_2nd_t, p1_de_serve_2nd_body, p1_de_serve_2nd_wide])
        x = x.replace('"p1_de_serve_2nd_t"', '%d%%'%rounded[0])
        x = x.replace('"p1_de_serve_2nd_t_size"', region_size(rounded[0]))
        x = x.replace('"p1_de_serve_2nd_body"', '%d%%'%rounded[1])
        x = x.replace('"p1_de_serve_2nd_body_size"', region_size(rounded[1]))
        x = x.replace('"p1_de_serve_2nd_wide"', '%d%%'%rounded[2])
        x = x.replace('"p1_de_serve_2nd_wide_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_de_serve_2nd_t"', '')
        x = x.replace('"p1_de_serve_2nd_t_size"', "mid")
        x = x.replace('"p1_de_serve_2nd_body"', '')
        x = x.replace('"p1_de_serve_2nd_body_size"', "mid")
        x = x.replace('"p1_de_serve_2nd_wide"', '')
        x = x.replace('"p1_de_serve_2nd_wide_size"', "mid")
    
    if (p1_serve_1st_t_15_40+p1_serve_1st_body_15_40+p1_serve_1st_wide_15_40)>0:
        rounded = LargestRemainderRound([p1_serve_1st_t_15_40, p1_serve_1st_body_15_40, p1_serve_1st_wide_15_40])
        x = x.replace('"p1_serve_1st_t_15_40"', '%d%%'%rounded[0])
        x = x.replace('"p1_serve_1st_body_15_40"', '%d%%'%rounded[1])
        x = x.replace('"p1_serve_1st_wide_15_40"', '%d%%'%rounded[2])
    else:
        x = x.replace('"p1_serve_1st_t_15_40"', '')
        x = x.replace('"p1_serve_1st_body_15_40"', '')
        x = x.replace('"p1_serve_1st_wide_15_40"', '')
    
    if (p1_serve_2nd_t_15_40+p1_serve_2nd_body_15_40+p1_serve_2nd_wide_15_40)>0:
        rounded = LargestRemainderRound([p1_serve_2nd_t_15_40, p1_serve_2nd_body_15_40, p1_serve_2nd_wide_15_40])
        x = x.replace('"p1_serve_2nd_t_15_40"', '%d%%'%rounded[0])
        x = x.replace('"p1_serve_2nd_body_15_40"', '%d%%'%rounded[1])
        x = x.replace('"p1_serve_2nd_wide_15_40"', '%d%%'%rounded[2])
    else:
        x = x.replace('"p1_serve_2nd_t_15_40"', '')
        x = x.replace('"p1_serve_2nd_body_15_40"', '')
        x = x.replace('"p1_serve_2nd_wide_15_40"', '')
    
    if (p1_serve_1st_t_30_30+p1_serve_1st_body_30_30+p1_serve_1st_wide_30_30)>0:
        rounded = LargestRemainderRound([p1_serve_1st_t_30_30, p1_serve_1st_body_30_30, p1_serve_1st_wide_30_30])
        x = x.replace('"p1_serve_1st_t_30_30"', '%d%%'%rounded[0])
        x = x.replace('"p1_serve_1st_body_30_30"', '%d%%'%rounded[1])
        x = x.replace('"p1_serve_1st_wide_30_30"', '%d%%'%rounded[2])
    else:
        x = x.replace('"p1_serve_1st_t_30_30"', '')
        x = x.replace('"p1_serve_1st_body_30_30"', '')
        x = x.replace('"p1_serve_1st_wide_30_30"', '')
    
    if (p1_serve_2nd_t_30_30+p1_serve_2nd_body_30_30+p1_serve_2nd_wide_30_30)>0:
        rounded = LargestRemainderRound([p1_serve_2nd_t_30_30, p1_serve_2nd_body_30_30, p1_serve_2nd_wide_30_30])
        x = x.replace('"p1_serve_2nd_t_30_30"', '%d%%'%rounded[0])
        x = x.replace('"p1_serve_2nd_body_30_30"', '%d%%'%rounded[1])
        x = x.replace('"p1_serve_2nd_wide_30_30"', '%d%%'%rounded[2])
    else:
        x = x.replace('"p1_serve_2nd_t_30_30"', '')
        x = x.replace('"p1_serve_2nd_body_30_30"', '')
        x = x.replace('"p1_serve_2nd_wide_30_30"', '')
    
    if (p1_serve_1st_wide_30_40+p1_serve_1st_body_30_40+p1_serve_1st_t_30_40)>0:
        rounded = LargestRemainderRound([p1_serve_1st_wide_30_40, p1_serve_1st_body_30_40, p1_serve_1st_t_30_40])
        x = x.replace('"p1_serve_1st_wide_30_40"', '%d%%'%rounded[0])
        x = x.replace('"p1_serve_1st_body_30_40"', '%d%%'%rounded[1])
        x = x.replace('"p1_serve_1st_t_30_40"', '%d%%'%rounded[2])
    else:
        x = x.replace('"p1_serve_1st_wide_30_40"', '')
        x = x.replace('"p1_serve_1st_body_30_40"', '')
        x = x.replace('"p1_serve_1st_t_30_40"', '')
    
    if (p1_serve_2nd_wide_30_40+p1_serve_2nd_body_30_40+p1_serve_2nd_t_30_40)>0:
        rounded = LargestRemainderRound([p1_serve_2nd_wide_30_40, p1_serve_2nd_body_30_40, p1_serve_2nd_t_30_40])
        x = x.replace('"p1_serve_2nd_wide_30_40"', '%d%%'%rounded[0])
        x = x.replace('"p1_serve_2nd_body_30_40"', '%d%%'%rounded[1])
        x = x.replace('"p1_serve_2nd_t_30_40"', '%d%%'%rounded[2])
    else:
        x = x.replace('"p1_serve_2nd_wide_30_40"', '')
        x = x.replace('"p1_serve_2nd_body_30_40"', '')
        x = x.replace('"p1_serve_2nd_t_30_40"', '')
    
    if (p1_serve_1st_wide_40_ad+p1_serve_1st_body_40_ad+p1_serve_1st_t_40_ad)>0:
        rounded = LargestRemainderRound([p1_serve_1st_wide_40_ad, p1_serve_1st_body_40_ad, p1_serve_1st_t_40_ad])
        x = x.replace('"p1_serve_1st_wide_40_ad"', '%d%%'%rounded[0])
        x = x.replace('"p1_serve_1st_body_40_ad"', '%d%%'%rounded[1])
        x = x.replace('"p1_serve_1st_t_40_ad"', '%d%%'%rounded[2])
    else:
        x = x.replace('"p1_serve_1st_wide_40_ad"', '')
        x = x.replace('"p1_serve_1st_body_40_ad"', '')
        x = x.replace('"p1_serve_1st_t_40_ad"', '')
    
    if (p1_serve_2nd_wide_40_ad+p1_serve_2nd_body_40_ad+p1_serve_2nd_t_40_ad)>0:
        rounded = LargestRemainderRound([p1_serve_2nd_wide_40_ad, p1_serve_2nd_body_40_ad, p1_serve_2nd_t_40_ad])
        x = x.replace('"p1_serve_2nd_wide_40_ad"', '%d%%'%rounded[0])
        x = x.replace('"p1_serve_2nd_body_40_ad"', '%d%%'%rounded[1])
        x = x.replace('"p1_serve_2nd_t_40_ad"', '%d%%'%rounded[2])
    else:
        x = x.replace('"p1_serve_2nd_wide_40_ad"', '')
        x = x.replace('"p1_serve_2nd_body_40_ad"', '')
        x = x.replace('"p1_serve_2nd_t_40_ad"', '')

    if (p1_serve_1st_wide_break_point+p1_serve_1st_body_break_point+p1_serve_1st_t_break_point)>0:
        rounded = LargestRemainderRound([p1_serve_1st_wide_break_point, p1_serve_1st_body_break_point, p1_serve_1st_t_break_point])
        x = x.replace('"p1_serve_1st_wide_break_point"', '%d%%'%rounded[0])
        x = x.replace('"p1_serve_1st_wide_break_point_size"', region_size(rounded[0]))
        x = x.replace('"p1_serve_1st_body_break_point"', '%d%%'%rounded[1])
        x = x.replace('"p1_serve_1st_body_break_point_size"', region_size(rounded[1]))
        x = x.replace('"p1_serve_1st_t_break_point"', '%d%%'%rounded[2])
        x = x.replace('"p1_serve_1st_t_break_point_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_serve_1st_wide_break_point"', '')
        x = x.replace('"p1_serve_1st_wide_break_point_size"', "mid")
        x = x.replace('"p1_serve_1st_body_break_point"', '')
        x = x.replace('"p1_serve_1st_body_break_point_size"', "mid")
        x = x.replace('"p1_serve_1st_t_break_point"', '')
        x = x.replace('"p1_serve_1st_t_break_point_size"', "mid")

    if (p1_serve_2nd_wide_break_point+p1_serve_2nd_body_break_point+p1_serve_2nd_t_break_point)>0:
        rounded = LargestRemainderRound([p1_serve_2nd_wide_break_point, p1_serve_2nd_body_break_point, p1_serve_2nd_t_break_point])
        x = x.replace('"p1_serve_2nd_wide_break_point"', '%d%%'%rounded[0])
        x = x.replace('"p1_serve_2nd_wide_break_point_size"', region_size(rounded[0]))
        x = x.replace('"p1_serve_2nd_body_break_point"', '%d%%'%rounded[1])
        x = x.replace('"p1_serve_2nd_body_break_point_size"', region_size(rounded[1]))
        x = x.replace('"p1_serve_2nd_t_break_point"', '%d%%'%rounded[2])
        x = x.replace('"p1_serve_2nd_t_break_point_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_serve_2nd_wide_break_point"', '')
        x = x.replace('"p1_serve_2nd_wide_break_point_size"', "mid")
        x = x.replace('"p1_serve_2nd_body_break_point"', '')
        x = x.replace('"p1_serve_2nd_body_break_point_size"', "mid")
        x = x.replace('"p1_serve_2nd_t_break_point"', '')
        x = x.replace('"p1_serve_2nd_t_break_point_size"', "mid")
        
    if (pl_return_fh_left+p1_return_fh_mid+p1_return_fh_right)>0:
        rounded = LargestRemainderRound([pl_return_fh_left, p1_return_fh_mid, p1_return_fh_right])
        x = x.replace('"pl_return_fh_left"', '%d%%'%rounded[0])
        x = x.replace('"p1_return_fh_mid"', '%d%%'%rounded[1])
        x = x.replace('"p1_return_fh_right"', '%d%%'%rounded[2])
    else:
        x = x.replace('"pl_return_fh_left"', '')
        x = x.replace('"p1_return_fh_mid"', '')
        x = x.replace('"p1_return_fh_right"', '')
        
    if (pl_return_bh_left+p1_return_bh_mid+p1_return_bh_right)>0:
        rounded = LargestRemainderRound([pl_return_bh_left, p1_return_bh_mid, p1_return_bh_right])
        x = x.replace('"pl_return_bh_left"', '%d%%'%rounded[0])
        x = x.replace('"p1_return_bh_mid"', '%d%%'%rounded[1])
        x = x.replace('"p1_return_bh_right"', '%d%%'%rounded[2])
    else:
        x = x.replace('"pl_return_bh_left"', '')
        x = x.replace('"p1_return_bh_mid"', '')
        x = x.replace('"p1_return_bh_right"', '')
    
    if (pl_return_body_left+p1_return_body_mid+p1_return_body_right)>0:
        rounded = LargestRemainderRound([pl_return_body_left, p1_return_body_mid, p1_return_body_right])
        x = x.replace('"pl_return_body_left"', '%d%%'%rounded[0])
        x = x.replace('"p1_return_body_mid"', '%d%%'%rounded[1])
        x = x.replace('"p1_return_body_right"', '%d%%'%rounded[2])
    else:
        x = x.replace('"pl_return_body_left"', '')
        x = x.replace('"p1_return_body_mid"', '')
        x = x.replace('"p1_return_body_right"', '')
        
    if (pl_return_fh_shallow+p1_return_fh_deep)>0:
        x = x.replace('"pl_return_fh_shallow"', '{:.0%}'.format(pl_return_fh_shallow/(pl_return_fh_shallow+p1_return_fh_deep)))
        x = x.replace('"p1_return_fh_deep"', '{:.0%}'.format(p1_return_fh_deep/(pl_return_fh_shallow+p1_return_fh_deep)))
    else:
        x = x.replace('"pl_return_fh_shallow"', '')
        x = x.replace('"p1_return_fh_deep"', '')
        
    if (pl_return_bh_shallow+p1_return_bh_deep)>0:
        x = x.replace('"pl_return_bh_shallow"', '{:.0%}'.format(pl_return_bh_shallow/(pl_return_bh_shallow+p1_return_bh_deep)))
        x = x.replace('"p1_return_bh_deep"', '{:.0%}'.format(p1_return_bh_deep/(pl_return_bh_shallow+p1_return_bh_deep)))
    else:
        x = x.replace('"pl_return_bh_shallow"', '')
        x = x.replace('"p1_return_bh_deep"', '')

    if (pl_return_body_shallow+p1_return_body_deep)>0:
        x = x.replace('"pl_return_body_shallow"', '{:.0%}'.format(pl_return_body_shallow/(pl_return_body_shallow+p1_return_body_deep)))
        x = x.replace('"p1_return_body_deep"', '{:.0%}'.format(p1_return_body_deep/(pl_return_body_shallow+p1_return_body_deep)))
    else:
        x = x.replace('"pl_return_body_shallow"', '')
        x = x.replace('"p1_return_body_deep"', '')
        
    if (oppo_serve_ace_fh+oppo_serve_winner_fh+oppo_serve_in_fh)>0:
        x = x.replace('"pl_return_fh_error"', '{:.0%}'.format(pl_return_fh_error/(oppo_serve_ace_fh+oppo_serve_winner_fh+oppo_serve_in_fh)))
        x = x.replace('"oppo_serve_ace_fh"', '{:.0%}'.format(oppo_serve_ace_fh/(oppo_serve_ace_fh+oppo_serve_winner_fh+oppo_serve_in_fh)))
    else:
        x = x.replace('"pl_return_fh_error"', '')
        x = x.replace('"oppo_serve_ace_fh"', '')
        
    if (oppo_serve_ace_bh+oppo_serve_winner_bh+oppo_serve_in_bh)>0:
        x = x.replace('"pl_return_bh_error"', '{:.0%}'.format(pl_return_bh_error/(oppo_serve_ace_bh+oppo_serve_winner_bh+oppo_serve_in_bh)))
        x = x.replace('"oppo_serve_ace_bh"', '{:.0%}'.format(oppo_serve_ace_bh/(oppo_serve_ace_bh+oppo_serve_winner_bh+oppo_serve_in_bh)))
    else:
        x = x.replace('"pl_return_bh_error"', '')
        x = x.replace('"oppo_serve_ace_bh"', '')
        
    if (oppo_serve_ace_body+oppo_serve_winner_body+oppo_serve_in_body)>0:
        x = x.replace('"pl_return_body_error"', '{:.0%}'.format(pl_return_body_error/(oppo_serve_ace_body+oppo_serve_winner_body+oppo_serve_in_body)))
        x = x.replace('"oppo_serve_ace_body"', '{:.0%}'.format(oppo_serve_ace_body/(oppo_serve_ace_body+oppo_serve_winner_body+oppo_serve_in_body)))
    else:
        x = x.replace('"pl_return_body_error"', '')
        x = x.replace('"oppo_serve_ace_body"', '')

    out_lines.append(x)
    
output = open(report_page1_name, 'w')
output.writelines(out_lines)
output.close()

# The next step is to find a list of matches, which provides source data for the report
start_date = (add_years(datetime.strptime(target_match_date, '%Y%m%d').date(), -1)).strftime('%Y%m%d')
target_match_date = datetime.strptime(target_match_date, '%Y%m%d').strftime('%Y%m%d')
if player1_name != "All Right-Hand" and player1_name != "All Left-Hand" \
        and player2_name != "All Right-Hand" and player2_name != "All Left-Hand":
    hist_matches = match_all[(match_all.match_id < target_match_date)
                             & (match_all.match_id > start_date)
                             & (((match_all.player1 == player1_name) & (match_all.player2 == player2_name)
                                 | (match_all.player2 == player1_name) & (match_all.player1 == player2_name)))]
elif player2_name == "All Right-Hand":
    hist_matches = match_all[(match_all.match_id < target_match_date)
                             & (match_all.match_id > start_date)
                             & (((match_all.player1 == player1_name) & (match_all.player2hand == "RH")
                                 | (match_all.player2 == player1_name) & (match_all.player1hand == "RH")))]
elif player2_name == "All Left-Hand":
    hist_matches = match_all[(match_all.match_id < target_match_date)
                             & (match_all.match_id > start_date)
                             & (((match_all.player1 == player1_name) & (match_all.player2hand == "LH")
                                 | (match_all.player2 == player1_name) & (match_all.player1hand == "LH")))]
# elif player1_name == "All Right-Hand":
#     hist_matches = match_all[(match_all.match_id < target_match_date)
#                             &(match_all.match_id > start_date)
#                             &( ( (match_all.player1 == player2_name) & (match_all.player2hand == "RH")
#                                | (match_all.player2 == player2_name) & (match_all.player1hand == "RH") ) )]
# elif player1_name == "All Left-Hand":
#     hist_matches = match_all[(match_all.match_id < target_match_date)
#                             &(match_all.match_id > start_date)
#                             &( ( (match_all.player1 == player2_name) & (match_all.player2hand == "LH")
#                                | (match_all.player2 == player2_name) & (match_all.player1hand == "LH") ) )]

df_source = df_all[df_all[47].isin(hist_matches.match_id)]

# player1 deuce court 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[13] == 1) & (df_source[12] == 1)][15].value_counts()
p1_de_serve_1st_t = temp_data[6] if 6 in temp_data.index else 0
p1_de_serve_1st_wide = temp_data[4] if 4 in temp_data.index else 0
p1_de_serve_1st_body = temp_data[5] if 5 in temp_data.index else 0
print("player1 deuce court 1st serve to (t, body, wide):", p1_de_serve_1st_t, p1_de_serve_1st_body,
      p1_de_serve_1st_wide)

# player1 AD court 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[13] == 3) & (df_source[12] == 1)][15].value_counts()
p1_ad_serve_1st_t = temp_data[6] if 6 in temp_data.index else 0
p1_ad_serve_1st_wide = temp_data[4] if 4 in temp_data.index else 0
p1_ad_serve_1st_body = temp_data[5] if 5 in temp_data.index else 0
print("player1 ad court 1st serve to (t, body, wide):", p1_ad_serve_1st_t, p1_ad_serve_1st_body, p1_ad_serve_1st_wide)

# player1 deuce court 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[13] == 1) & (df_source[12] == 2)][15].value_counts()
p1_de_serve_2nd_t = temp_data[6] if 6 in temp_data.index else 0
p1_de_serve_2nd_wide = temp_data[4] if 4 in temp_data.index else 0
p1_de_serve_2nd_body = temp_data[5] if 5 in temp_data.index else 0
print("player1 deuce court 2nd serve to (t, body, wide):", p1_de_serve_2nd_t, p1_de_serve_2nd_body,
      p1_de_serve_2nd_wide)

# player1 AD court 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[13] == 3) & (df_source[12] == 2)][15].value_counts()
p1_ad_serve_2nd_t = temp_data[6] if 6 in temp_data.index else 0
p1_ad_serve_2nd_wide = temp_data[4] if 4 in temp_data.index else 0
p1_ad_serve_2nd_body = temp_data[5] if 5 in temp_data.index else 0
print("player1 ad court 2nd serve to (t, body, wide):", p1_ad_serve_2nd_t, p1_ad_serve_2nd_body, p1_ad_serve_2nd_wide)

# player1 15-40 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12] == 1)
                      & (df_source[4] == 1) & (df_source[5] == 3)][15].value_counts()
p1_serve_1st_t_15_40 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_1st_wide_15_40 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_1st_body_15_40 = temp_data[5] if 5 in temp_data.index else 0
print("player1 15-40 1st serve to (t, body, wide):", p1_serve_1st_t_15_40, p1_serve_1st_body_15_40,
      p1_serve_1st_wide_15_40)

# player1 15-40 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12] == 2)
                      & (df_source[4] == 1) & (df_source[5] == 3)][15].value_counts()
p1_serve_2nd_t_15_40 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_2nd_wide_15_40 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_2nd_body_15_40 = temp_data[5] if 5 in temp_data.index else 0
print("player1 15-40 2nd serve to (t, body, wide):", p1_serve_2nd_t_15_40, p1_serve_2nd_body_15_40,
      p1_serve_2nd_wide_15_40)

# player1 30-30 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12] == 1)
                      & (df_source[4] == 2) & (df_source[5] == 2)][15].value_counts()
p1_serve_1st_t_30_30 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_1st_wide_30_30 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_1st_body_30_30 = temp_data[5] if 5 in temp_data.index else 0
print("player1 30-30 1st serve to (t, body, wide):", p1_serve_1st_t_30_30, p1_serve_1st_body_30_30,
      p1_serve_1st_wide_30_30)

# player1 30-30 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12] == 2)
                      & (df_source[4] == 2) & (df_source[5] == 2)][15].value_counts()
p1_serve_2nd_t_30_30 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_2nd_wide_30_30 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_2nd_body_30_30 = temp_data[5] if 5 in temp_data.index else 0
print("player1 30-30 2nd serve to (t, body, wide):", p1_serve_2nd_t_30_30, p1_serve_2nd_body_30_30,
      p1_serve_2nd_wide_30_30)

# player1 30-40 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12] == 1)
                      & (df_source[4] == 2) & (df_source[5] == 3)][15].value_counts()
p1_serve_1st_t_30_40 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_1st_wide_30_40 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_1st_body_30_40 = temp_data[5] if 5 in temp_data.index else 0
print("player1 30-40 1st serve to (t, body, wide):", p1_serve_1st_t_30_40, p1_serve_1st_body_30_40,
      p1_serve_1st_wide_30_40)

# player1 30-40 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12] == 2)
                      & (df_source[4] == 2) & (df_source[5] == 3)][15].value_counts()
p1_serve_2nd_t_30_40 = temp_data[6] if 6 in temp_data.index else 0
p1_serve_2nd_wide_30_40 = temp_data[4] if 4 in temp_data.index else 0
p1_serve_2nd_body_30_40 = temp_data[5] if 5 in temp_data.index else 0
print("player1 30-40 2nd serve to (t, body, wide):", p1_serve_2nd_t_30_40, p1_serve_2nd_body_30_40,
      p1_serve_2nd_wide_30_40)

# player1 40-AD 1st serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12] == 1)
                      & (df_source[4] == 3) & (df_source[5] == 4)][15].value_counts()
p1_serve_1st_t_40_ad = temp_data[6] if 6 in temp_data.index else 0
p1_serve_1st_wide_40_ad = temp_data[4] if 4 in temp_data.index else 0
p1_serve_1st_body_40_ad = temp_data[5] if 5 in temp_data.index else 0
print("player1 40-AD 1st serve to (t, body, wide):", p1_serve_1st_t_40_ad, p1_serve_1st_body_40_ad,
      p1_serve_1st_wide_40_ad)

# player1 40-AD 2nd serve direction count
temp_data = df_source[(df_source[0] == player1_name) & (df_source[12] == 2)
                      & (df_source[4] == 3) & (df_source[5] == 4)][15].value_counts()
p1_serve_2nd_t_40_ad = temp_data[6] if 6 in temp_data.index else 0
p1_serve_2nd_wide_40_ad = temp_data[4] if 4 in temp_data.index else 0
p1_serve_2nd_body_40_ad = temp_data[5] if 5 in temp_data.index else 0
print("player1 40-AD 2nd serve to (t, body, wide):", p1_serve_2nd_t_40_ad, p1_serve_2nd_body_40_ad,
      p1_serve_2nd_wide_40_ad)

p1_serve_1st_wide_break_point = p1_serve_1st_wide_30_40 + p1_serve_1st_wide_40_ad
p1_serve_1st_body_break_point = p1_serve_1st_body_30_40 + p1_serve_1st_body_40_ad
p1_serve_1st_t_break_point = p1_serve_1st_t_30_40 + p1_serve_1st_t_40_ad
print("player1 break point 1st serve to (t, body, wide):", p1_serve_1st_t_break_point, p1_serve_1st_body_break_point,
      p1_serve_1st_wide_break_point)

p1_serve_2nd_wide_break_point = p1_serve_2nd_wide_30_40 + p1_serve_2nd_wide_40_ad
p1_serve_2nd_body_break_point = p1_serve_2nd_body_30_40 + p1_serve_2nd_body_40_ad
p1_serve_2nd_t_break_point = p1_serve_2nd_t_30_40 + p1_serve_2nd_t_40_ad
print("player1 break point 2nd serve to (t, body, wide):", p1_serve_2nd_t_break_point, p1_serve_2nd_body_break_point,
      p1_serve_2nd_wide_break_point)

df_return = df_source[(df_source[0] == player1_name) & (df_source[12] == 3)]

# player1 forehand return direction count
if df_return.iloc[0][2] == "RH":  # player1 is RH
    temp_data = df_return[((df_return[26] == 4) & (df_return[24] == 1))
                          | ((df_return[26] == 6) & (df_return[24] == 3))][16].value_counts()
    pl_return_fh_left = temp_data[1] if 1 in temp_data.index else 0
    p1_return_fh_mid = temp_data[2] if 2 in temp_data.index else 0
    p1_return_fh_right = temp_data[3] if 3 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26] == 6) & (df_return[24] == 1))
                          | ((df_return[26] == 4) & (df_return[24] == 3))][16].value_counts()
    pl_return_fh_left = temp_data[1] if 1 in temp_data.index else 0
    p1_return_fh_mid = temp_data[2] if 2 in temp_data.index else 0
    p1_return_fh_right = temp_data[3] if 3 in temp_data.index else 0
print("player1 forehand return to (left, middle, right):", pl_return_fh_left, p1_return_fh_mid, p1_return_fh_right)

# player1 backhand return direction count
if df_return.iloc[0][2] == "RH":  # player1 is RH
    temp_data = df_return[((df_return[26] == 6) & (df_return[24] == 1))
                          | ((df_return[26] == 4) & (df_return[24] == 3))][16].value_counts()
    pl_return_bh_left = temp_data[1] if 1 in temp_data.index else 0
    p1_return_bh_mid = temp_data[2] if 2 in temp_data.index else 0
    p1_return_bh_right = temp_data[3] if 3 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26] == 4) & (df_return[24] == 1))
                          | ((df_return[26] == 6) & (df_return[24] == 3))][16].value_counts()
    pl_return_bh_left = temp_data[1] if 1 in temp_data.index else 0
    p1_return_bh_mid = temp_data[2] if 2 in temp_data.index else 0
    p1_return_bh_right = temp_data[3] if 3 in temp_data.index else 0
print("player1 backhand return to (left, middle, right):", pl_return_bh_left, p1_return_bh_mid, p1_return_bh_right)

# player1 body return direction count
temp_data = df_return[(df_return[26] == 5)][16].value_counts()
pl_return_body_left = temp_data[1] if 1 in temp_data.index else 0
p1_return_body_mid = temp_data[2] if 2 in temp_data.index else 0
p1_return_body_right = temp_data[3] if 3 in temp_data.index else 0
print("player1 body return to (left, middle, right):", pl_return_body_left, p1_return_body_mid, p1_return_body_right)

# player1 forehand return depth count
if df_return.iloc[0][2] == "RH":  # player1 is RH
    temp_data = df_return[((df_return[26] == 4) & (df_return[24] == 1))
                          | ((df_return[26] == 6) & (df_return[24] == 3))][17].value_counts()
    pl_return_fh_shallow = temp_data[1] if 1 in temp_data.index else 0
    p1_return_fh_deep = temp_data[2] if 2 in temp_data.index else 0
    p1_return_fh_deep += temp_data[3] if 3 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26] == 6) & (df_return[24] == 1))
                          | ((df_return[26] == 4) & (df_return[24] == 3))][17].value_counts()
    pl_return_fh_shallow = temp_data[1] if 1 in temp_data.index else 0
    p1_return_fh_deep = temp_data[2] if 2 in temp_data.index else 0
    p1_return_fh_deep += temp_data[3] if 3 in temp_data.index else 0
print("player1 forehand return to (shallow, deep):", pl_return_fh_shallow, p1_return_fh_deep)

# player1 backhand return depth count
if df_return.iloc[0][2] == "RH":  # player1 is RH
    temp_data = df_return[((df_return[26] == 6) & (df_return[24] == 1))
                          | ((df_return[26] == 4) & (df_return[24] == 3))][17].value_counts()
    pl_return_bh_shallow = temp_data[1] if 1 in temp_data.index else 0
    p1_return_bh_deep = temp_data[2] if 2 in temp_data.index else 0
    p1_return_bh_deep += temp_data[3] if 3 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26] == 4) & (df_return[24] == 1))
                          | ((df_return[26] == 6) & (df_return[24] == 3))][17].value_counts()
    pl_return_bh_shallow = temp_data[1] if 1 in temp_data.index else 0
    p1_return_bh_deep = temp_data[2] if 2 in temp_data.index else 0
    p1_return_bh_deep += temp_data[3] if 3 in temp_data.index else 0
print("player1 backhand return to (shallow, deep):", pl_return_bh_shallow, p1_return_bh_deep)

# player1 body return depth count
temp_data = df_return[(df_return[26] == 5)][17].value_counts()
pl_return_body_shallow = temp_data[1] if 1 in temp_data.index else 0
p1_return_body_deep = temp_data[2] if 2 in temp_data.index else 0
p1_return_body_deep += temp_data[3] if 3 in temp_data.index else 0
print("player1 body return to (shallow, deep):", pl_return_body_shallow, p1_return_body_deep)

# player1 forehand return error count
if df_return.iloc[0][2] == "RH":  # player1 is RH
    temp_data = df_return[((df_return[26] == 4) & (df_return[24] == 1))
                          | ((df_return[26] == 6) & (df_return[24] == 3))][21].value_counts()
    pl_return_fh_error = temp_data[3] if 3 in temp_data.index else 0
    pl_return_fh_error += temp_data[4] if 4 in temp_data.index else 0
    temp_data = df_source[(df_source[0] != player1_name)
                          & ((df_source[12] == 1) | (df_source[12] == 2))
                          & (((df_source[15] == 4) & (df_source[13] == 1))
                             | ((df_source[15] == 6) & (df_source[13] == 3)))][21].value_counts()
    oppo_serve_ace_fh = temp_data[1] if 1 in temp_data.index else 0
    oppo_serve_winner_fh = temp_data[6] if 6 in temp_data.index else 0
    oppo_serve_in_fh = temp_data[7] if 7 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26] == 6) & (df_return[24] == 1))
                          | ((df_return[26] == 4) & (df_return[24] == 3))][21].value_counts()
    pl_return_fh_error = temp_data[3] if 3 in temp_data.index else 0
    pl_return_fh_error += temp_data[4] if 4 in temp_data.index else 0
    temp_data = df_source[(df_source[0] != player1_name)
                          & ((df_source[12] == 1) | (df_source[12] == 2))
                          & (((df_source[15] == 6) & (df_source[13] == 1))
                             | ((df_source[15] == 4) & (df_source[13] == 3)))][21].value_counts()
    oppo_serve_ace_fh = temp_data[1] if 1 in temp_data.index else 0
    oppo_serve_winner_fh = temp_data[6] if 6 in temp_data.index else 0
    oppo_serve_in_fh = temp_data[7] if 7 in temp_data.index else 0
print("player1 forehand return (error, server ace, total):",
      pl_return_fh_error + oppo_serve_winner_fh, oppo_serve_ace_fh,
      oppo_serve_ace_fh + oppo_serve_winner_fh + oppo_serve_in_fh)

# player1 backhand return error count
if df_return.iloc[0][2] == "RH":  # player1 is RH
    temp_data = df_return[((df_return[26] == 6) & (df_return[24] == 1))
                          | ((df_return[26] == 4) & (df_return[24] == 3))][21].value_counts()
    pl_return_bh_error = temp_data[3] if 3 in temp_data.index else 0
    pl_return_bh_error += temp_data[4] if 4 in temp_data.index else 0
    temp_data = df_source[(df_source[0] != player1_name)
                          & ((df_source[12] == 1) | (df_source[12] == 2))
                          & (((df_source[15] == 6) & (df_source[13] == 1))
                             | ((df_source[15] == 4) & (df_source[13] == 3)))][21].value_counts()
    oppo_serve_ace_bh = temp_data[1] if 1 in temp_data.index else 0
    oppo_serve_winner_bh = temp_data[6] if 6 in temp_data.index else 0
    oppo_serve_in_bh = temp_data[7] if 7 in temp_data.index else 0
elif df_return.iloc[0][2] == "LH":
    temp_data = df_return[((df_return[26] == 4) & (df_return[24] == 1))
                          | ((df_return[26] == 6) & (df_return[24] == 3))][21].value_counts()
    pl_return_bh_error = temp_data[3] if 3 in temp_data.index else 0
    pl_return_bh_error += temp_data[4] if 4 in temp_data.index else 0
    temp_data = df_source[(df_source[0] != player1_name)
                          & ((df_source[12] == 1) | (df_source[12] == 2))
                          & (((df_source[15] == 4) & (df_source[13] == 1))
                             | ((df_source[15] == 6) & (df_source[13] == 3)))][21].value_counts()
    oppo_serve_ace_bh = temp_data[1] if 1 in temp_data.index else 0
    oppo_serve_winner_bh = temp_data[6] if 6 in temp_data.index else 0
    oppo_serve_in_bh = temp_data[7] if 7 in temp_data.index else 0
print("player1 backhand return (error, server ace, total):",
      pl_return_bh_error + oppo_serve_winner_bh, oppo_serve_ace_bh,
      oppo_serve_ace_bh + oppo_serve_winner_bh + oppo_serve_in_bh)

# player1 body return error count
temp_data = df_return[(df_return[26] == 5)][21].value_counts()
pl_return_body_error = temp_data[3] if 3 in temp_data.index else 0
pl_return_body_error += temp_data[4] if 4 in temp_data.index else 0
temp_data = df_source[(df_source[0] != player1_name)
                      & ((df_source[12] == 1) | (df_source[12] == 2))
                      & (df_source[15] == 5)][21].value_counts()
oppo_serve_ace_body = temp_data[1] if 1 in temp_data.index else 0
oppo_serve_winner_body = temp_data[6] if 6 in temp_data.index else 0
oppo_serve_in_body = temp_data[7] if 7 in temp_data.index else 0
print("player1 body return (error, server ace, total):",
      pl_return_body_error + oppo_serve_winner_body, oppo_serve_ace_body,
      oppo_serve_ace_body + oppo_serve_winner_body + oppo_serve_in_body)

template = open('tennis-data-report-template-v2-page2.html', 'r')
all_lines = template.readlines()
out_lines = []
for line in all_lines:
    x = line.replace('"p1_name"', player1_name)
    x = x.replace('"p1_short_name"', player1_name.split(' ')[1][:15])
    x = x.replace('"p2_name"', player2_name)
    x = x.replace('"p2_short_name"', player2_name.split(' ')[1][:15])
    x = x.replace('"start_date"', datetime.strptime(start_date, '%Y%m%d').strftime('%Y-%m-%d'))
    x = x.replace('"target_match_date"', datetime.strptime(target_match_date, '%Y%m%d').strftime('%Y-%m-%d'))
    x = x.replace('"page1.html"', report_page1_name)
    x = x.replace('"page2.html"', report_page2_name)
    #    x = x.replace('"p1_winning_chance"', '{:.1%}'.format(p1_winning_chance))

    if (p1_ad_serve_1st_wide + p1_ad_serve_1st_body + p1_ad_serve_1st_t) > 0:
        rounded = LargestRemainderRound([p1_ad_serve_1st_wide, p1_ad_serve_1st_body, p1_ad_serve_1st_t])
        x = x.replace('"p1_ad_serve_1st_wide"', '%d%%' % rounded[0])
        x = x.replace('"p1_ad_serve_1st_wide_size"', region_size(rounded[0]))
        x = x.replace('"p1_ad_serve_1st_body"', '%d%%' % rounded[1])
        x = x.replace('"p1_ad_serve_1st_body_size"', region_size(rounded[1]))
        x = x.replace('"p1_ad_serve_1st_t"', '%d%%' % rounded[2])
        x = x.replace('"p1_ad_serve_1st_t_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_ad_serve_1st_wide"', '')
        x = x.replace('"p1_ad_serve_1st_wide_size"', "mid")
        x = x.replace('"p1_ad_serve_1st_body"', '')
        x = x.replace('"p1_ad_serve_1st_body_size"', "mid")
        x = x.replace('"p1_ad_serve_1st_t"', '')
        x = x.replace('"p1_ad_serve_1st_t_size"', "mid")

    if (p1_de_serve_1st_t + p1_de_serve_1st_body + p1_de_serve_1st_wide) > 0:
        rounded = LargestRemainderRound([p1_de_serve_1st_t, p1_de_serve_1st_body, p1_de_serve_1st_wide])
        x = x.replace('"p1_de_serve_1st_t"', '%d%%' % rounded[0])
        x = x.replace('"p1_de_serve_1st_t_size"', region_size(rounded[0]))
        x = x.replace('"p1_de_serve_1st_body"', '%d%%' % rounded[1])
        x = x.replace('"p1_de_serve_1st_body_size"', region_size(rounded[1]))
        x = x.replace('"p1_de_serve_1st_wide"', '%d%%' % rounded[2])
        x = x.replace('"p1_de_serve_1st_wide_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_de_serve_1st_t"', '')
        x = x.replace('"p1_de_serve_1st_t_size"', "mid")
        x = x.replace('"p1_de_serve_1st_body"', '')
        x = x.replace('"p1_de_serve_1st_body_size"', "mid")
        x = x.replace('"p1_de_serve_1st_wide"', '')
        x = x.replace('"p1_de_serve_1st_wide_size"', "mid")

    if (p1_ad_serve_2nd_wide + p1_ad_serve_2nd_body + p1_ad_serve_2nd_t) > 0:
        rounded = LargestRemainderRound([p1_ad_serve_2nd_wide, p1_ad_serve_2nd_body, p1_ad_serve_2nd_t])
        x = x.replace('"p1_ad_serve_2nd_wide"', '%d%%' % rounded[0])
        x = x.replace('"p1_ad_serve_2nd_wide_size"', region_size(rounded[0]))
        x = x.replace('"p1_ad_serve_2nd_body"', '%d%%' % rounded[1])
        x = x.replace('"p1_ad_serve_2nd_body_size"', region_size(rounded[1]))
        x = x.replace('"p1_ad_serve_2nd_t"', '%d%%' % rounded[2])
        x = x.replace('"p1_ad_serve_2nd_t_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_ad_serve_2nd_wide"', '')
        x = x.replace('"p1_ad_serve_2nd_wide_size"', "mid")
        x = x.replace('"p1_ad_serve_2nd_body"', '')
        x = x.replace('"p1_ad_serve_2nd_body_size"', "mid")
        x = x.replace('"p1_ad_serve_2nd_t"', '')
        x = x.replace('"p1_ad_serve_2nd_t_size"', "mid")

    if (p1_de_serve_2nd_t + p1_de_serve_2nd_body + p1_de_serve_2nd_wide) > 0:
        rounded = LargestRemainderRound([p1_de_serve_2nd_t, p1_de_serve_2nd_body, p1_de_serve_2nd_wide])
        x = x.replace('"p1_de_serve_2nd_t"', '%d%%' % rounded[0])
        x = x.replace('"p1_de_serve_2nd_t_size"', region_size(rounded[0]))
        x = x.replace('"p1_de_serve_2nd_body"', '%d%%' % rounded[1])
        x = x.replace('"p1_de_serve_2nd_body_size"', region_size(rounded[1]))
        x = x.replace('"p1_de_serve_2nd_wide"', '%d%%' % rounded[2])
        x = x.replace('"p1_de_serve_2nd_wide_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_de_serve_2nd_t"', '')
        x = x.replace('"p1_de_serve_2nd_t_size"', "mid")
        x = x.replace('"p1_de_serve_2nd_body"', '')
        x = x.replace('"p1_de_serve_2nd_body_size"', "mid")
        x = x.replace('"p1_de_serve_2nd_wide"', '')
        x = x.replace('"p1_de_serve_2nd_wide_size"', "mid")

    if (p1_serve_1st_t_15_40 + p1_serve_1st_body_15_40 + p1_serve_1st_wide_15_40) > 0:
        rounded = LargestRemainderRound([p1_serve_1st_t_15_40, p1_serve_1st_body_15_40, p1_serve_1st_wide_15_40])
        x = x.replace('"p1_serve_1st_t_15_40"', '%d%%' % rounded[0])
        x = x.replace('"p1_serve_1st_body_15_40"', '%d%%' % rounded[1])
        x = x.replace('"p1_serve_1st_wide_15_40"', '%d%%' % rounded[2])
    else:
        x = x.replace('"p1_serve_1st_t_15_40"', '')
        x = x.replace('"p1_serve_1st_body_15_40"', '')
        x = x.replace('"p1_serve_1st_wide_15_40"', '')

    if (p1_serve_2nd_t_15_40 + p1_serve_2nd_body_15_40 + p1_serve_2nd_wide_15_40) > 0:
        rounded = LargestRemainderRound([p1_serve_2nd_t_15_40, p1_serve_2nd_body_15_40, p1_serve_2nd_wide_15_40])
        x = x.replace('"p1_serve_2nd_t_15_40"', '%d%%' % rounded[0])
        x = x.replace('"p1_serve_2nd_body_15_40"', '%d%%' % rounded[1])
        x = x.replace('"p1_serve_2nd_wide_15_40"', '%d%%' % rounded[2])
    else:
        x = x.replace('"p1_serve_2nd_t_15_40"', '')
        x = x.replace('"p1_serve_2nd_body_15_40"', '')
        x = x.replace('"p1_serve_2nd_wide_15_40"', '')

    if (p1_serve_1st_t_30_30 + p1_serve_1st_body_30_30 + p1_serve_1st_wide_30_30) > 0:
        rounded = LargestRemainderRound([p1_serve_1st_t_30_30, p1_serve_1st_body_30_30, p1_serve_1st_wide_30_30])
        x = x.replace('"p1_serve_1st_t_30_30"', '%d%%' % rounded[0])
        x = x.replace('"p1_serve_1st_body_30_30"', '%d%%' % rounded[1])
        x = x.replace('"p1_serve_1st_wide_30_30"', '%d%%' % rounded[2])
    else:
        x = x.replace('"p1_serve_1st_t_30_30"', '')
        x = x.replace('"p1_serve_1st_body_30_30"', '')
        x = x.replace('"p1_serve_1st_wide_30_30"', '')

    if (p1_serve_2nd_t_30_30 + p1_serve_2nd_body_30_30 + p1_serve_2nd_wide_30_30) > 0:
        rounded = LargestRemainderRound([p1_serve_2nd_t_30_30, p1_serve_2nd_body_30_30, p1_serve_2nd_wide_30_30])
        x = x.replace('"p1_serve_2nd_t_30_30"', '%d%%' % rounded[0])
        x = x.replace('"p1_serve_2nd_body_30_30"', '%d%%' % rounded[1])
        x = x.replace('"p1_serve_2nd_wide_30_30"', '%d%%' % rounded[2])
    else:
        x = x.replace('"p1_serve_2nd_t_30_30"', '')
        x = x.replace('"p1_serve_2nd_body_30_30"', '')
        x = x.replace('"p1_serve_2nd_wide_30_30"', '')

    if (p1_serve_1st_wide_30_40 + p1_serve_1st_body_30_40 + p1_serve_1st_t_30_40) > 0:
        rounded = LargestRemainderRound([p1_serve_1st_wide_30_40, p1_serve_1st_body_30_40, p1_serve_1st_t_30_40])
        x = x.replace('"p1_serve_1st_wide_30_40"', '%d%%' % rounded[0])
        x = x.replace('"p1_serve_1st_body_30_40"', '%d%%' % rounded[1])
        x = x.replace('"p1_serve_1st_t_30_40"', '%d%%' % rounded[2])
    else:
        x = x.replace('"p1_serve_1st_wide_30_40"', '')
        x = x.replace('"p1_serve_1st_body_30_40"', '')
        x = x.replace('"p1_serve_1st_t_30_40"', '')

    if (p1_serve_2nd_wide_30_40 + p1_serve_2nd_body_30_40 + p1_serve_2nd_t_30_40) > 0:
        rounded = LargestRemainderRound([p1_serve_2nd_wide_30_40, p1_serve_2nd_body_30_40, p1_serve_2nd_t_30_40])
        x = x.replace('"p1_serve_2nd_wide_30_40"', '%d%%' % rounded[0])
        x = x.replace('"p1_serve_2nd_body_30_40"', '%d%%' % rounded[1])
        x = x.replace('"p1_serve_2nd_t_30_40"', '%d%%' % rounded[2])
    else:
        x = x.replace('"p1_serve_2nd_wide_30_40"', '')
        x = x.replace('"p1_serve_2nd_body_30_40"', '')
        x = x.replace('"p1_serve_2nd_t_30_40"', '')

    if (p1_serve_1st_wide_40_ad + p1_serve_1st_body_40_ad + p1_serve_1st_t_40_ad) > 0:
        rounded = LargestRemainderRound([p1_serve_1st_wide_40_ad, p1_serve_1st_body_40_ad, p1_serve_1st_t_40_ad])
        x = x.replace('"p1_serve_1st_wide_40_ad"', '%d%%' % rounded[0])
        x = x.replace('"p1_serve_1st_body_40_ad"', '%d%%' % rounded[1])
        x = x.replace('"p1_serve_1st_t_40_ad"', '%d%%' % rounded[2])
    else:
        x = x.replace('"p1_serve_1st_wide_40_ad"', '')
        x = x.replace('"p1_serve_1st_body_40_ad"', '')
        x = x.replace('"p1_serve_1st_t_40_ad"', '')

    if (p1_serve_2nd_wide_40_ad + p1_serve_2nd_body_40_ad + p1_serve_2nd_t_40_ad) > 0:
        rounded = LargestRemainderRound([p1_serve_2nd_wide_40_ad, p1_serve_2nd_body_40_ad, p1_serve_2nd_t_40_ad])
        x = x.replace('"p1_serve_2nd_wide_40_ad"', '%d%%' % rounded[0])
        x = x.replace('"p1_serve_2nd_body_40_ad"', '%d%%' % rounded[1])
        x = x.replace('"p1_serve_2nd_t_40_ad"', '%d%%' % rounded[2])
    else:
        x = x.replace('"p1_serve_2nd_wide_40_ad"', '')
        x = x.replace('"p1_serve_2nd_body_40_ad"', '')
        x = x.replace('"p1_serve_2nd_t_40_ad"', '')

    if (p1_serve_1st_wide_break_point + p1_serve_1st_body_break_point + p1_serve_1st_t_break_point) > 0:
        rounded = LargestRemainderRound(
            [p1_serve_1st_wide_break_point, p1_serve_1st_body_break_point, p1_serve_1st_t_break_point])
        x = x.replace('"p1_serve_1st_wide_break_point"', '%d%%' % rounded[0])
        x = x.replace('"p1_serve_1st_wide_break_point_size"', region_size(rounded[0]))
        x = x.replace('"p1_serve_1st_body_break_point"', '%d%%' % rounded[1])
        x = x.replace('"p1_serve_1st_body_break_point_size"', region_size(rounded[1]))
        x = x.replace('"p1_serve_1st_t_break_point"', '%d%%' % rounded[2])
        x = x.replace('"p1_serve_1st_t_break_point_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_serve_1st_wide_break_point"', '')
        x = x.replace('"p1_serve_1st_wide_break_point_size"', "mid")
        x = x.replace('"p1_serve_1st_body_break_point"', '')
        x = x.replace('"p1_serve_1st_body_break_point_size"', "mid")
        x = x.replace('"p1_serve_1st_t_break_point"', '')
        x = x.replace('"p1_serve_1st_t_break_point_size"', "mid")

    if (p1_serve_2nd_wide_break_point + p1_serve_2nd_body_break_point + p1_serve_2nd_t_break_point) > 0:
        rounded = LargestRemainderRound(
            [p1_serve_2nd_wide_break_point, p1_serve_2nd_body_break_point, p1_serve_2nd_t_break_point])
        x = x.replace('"p1_serve_2nd_wide_break_point"', '%d%%' % rounded[0])
        x = x.replace('"p1_serve_2nd_wide_break_point_size"', region_size(rounded[0]))
        x = x.replace('"p1_serve_2nd_body_break_point"', '%d%%' % rounded[1])
        x = x.replace('"p1_serve_2nd_body_break_point_size"', region_size(rounded[1]))
        x = x.replace('"p1_serve_2nd_t_break_point"', '%d%%' % rounded[2])
        x = x.replace('"p1_serve_2nd_t_break_point_size"', region_size(rounded[2]))
    else:
        x = x.replace('"p1_serve_2nd_wide_break_point"', '')
        x = x.replace('"p1_serve_2nd_wide_break_point_size"', "mid")
        x = x.replace('"p1_serve_2nd_body_break_point"', '')
        x = x.replace('"p1_serve_2nd_body_break_point_size"', "mid")
        x = x.replace('"p1_serve_2nd_t_break_point"', '')
        x = x.replace('"p1_serve_2nd_t_break_point_size"', "mid")

    if (pl_return_fh_left + p1_return_fh_mid + p1_return_fh_right) > 0:
        rounded = LargestRemainderRound([pl_return_fh_left, p1_return_fh_mid, p1_return_fh_right])
        x = x.replace('"pl_return_fh_left"', '%d%%' % rounded[0])
        x = x.replace('"p1_return_fh_mid"', '%d%%' % rounded[1])
        x = x.replace('"p1_return_fh_right"', '%d%%' % rounded[2])
    else:
        x = x.replace('"pl_return_fh_left"', '')
        x = x.replace('"p1_return_fh_mid"', '')
        x = x.replace('"p1_return_fh_right"', '')

    if (pl_return_bh_left + p1_return_bh_mid + p1_return_bh_right) > 0:
        rounded = LargestRemainderRound([pl_return_bh_left, p1_return_bh_mid, p1_return_bh_right])
        x = x.replace('"pl_return_bh_left"', '%d%%' % rounded[0])
        x = x.replace('"p1_return_bh_mid"', '%d%%' % rounded[1])
        x = x.replace('"p1_return_bh_right"', '%d%%' % rounded[2])
    else:
        x = x.replace('"pl_return_bh_left"', '')
        x = x.replace('"p1_return_bh_mid"', '')
        x = x.replace('"p1_return_bh_right"', '')

    if (pl_return_body_left + p1_return_body_mid + p1_return_body_right) > 0:
        rounded = LargestRemainderRound([pl_return_body_left, p1_return_body_mid, p1_return_body_right])
        x = x.replace('"pl_return_body_left"', '%d%%' % rounded[0])
        x = x.replace('"p1_return_body_mid"', '%d%%' % rounded[1])
        x = x.replace('"p1_return_body_right"', '%d%%' % rounded[2])
    else:
        x = x.replace('"pl_return_body_left"', '')
        x = x.replace('"p1_return_body_mid"', '')
        x = x.replace('"p1_return_body_right"', '')

    if (pl_return_fh_shallow + p1_return_fh_deep) > 0:
        x = x.replace('"pl_return_fh_shallow"',
                      '{:.0%}'.format(pl_return_fh_shallow / (pl_return_fh_shallow + p1_return_fh_deep)))
        x = x.replace('"p1_return_fh_deep"',
                      '{:.0%}'.format(p1_return_fh_deep / (pl_return_fh_shallow + p1_return_fh_deep)))
    else:
        x = x.replace('"pl_return_fh_shallow"', '')
        x = x.replace('"p1_return_fh_deep"', '')

    if (pl_return_bh_shallow + p1_return_bh_deep) > 0:
        x = x.replace('"pl_return_bh_shallow"',
                      '{:.0%}'.format(pl_return_bh_shallow / (pl_return_bh_shallow + p1_return_bh_deep)))
        x = x.replace('"p1_return_bh_deep"',
                      '{:.0%}'.format(p1_return_bh_deep / (pl_return_bh_shallow + p1_return_bh_deep)))
    else:
        x = x.replace('"pl_return_bh_shallow"', '')
        x = x.replace('"p1_return_bh_deep"', '')

    if (pl_return_body_shallow + p1_return_body_deep) > 0:
        x = x.replace('"pl_return_body_shallow"',
                      '{:.0%}'.format(pl_return_body_shallow / (pl_return_body_shallow + p1_return_body_deep)))
        x = x.replace('"p1_return_body_deep"',
                      '{:.0%}'.format(p1_return_body_deep / (pl_return_body_shallow + p1_return_body_deep)))
    else:
        x = x.replace('"pl_return_body_shallow"', '')
        x = x.replace('"p1_return_body_deep"', '')

    if (oppo_serve_ace_fh + oppo_serve_winner_fh + oppo_serve_in_fh) > 0:
        x = x.replace('"pl_return_fh_error"', '{:.0%}'.format(
            pl_return_fh_error / (oppo_serve_ace_fh + oppo_serve_winner_fh + oppo_serve_in_fh)))
        x = x.replace('"oppo_serve_ace_fh"', '{:.0%}'.format(
            oppo_serve_ace_fh / (oppo_serve_ace_fh + oppo_serve_winner_fh + oppo_serve_in_fh)))
    else:
        x = x.replace('"pl_return_fh_error"', '')
        x = x.replace('"oppo_serve_ace_fh"', '')

    if (oppo_serve_ace_bh + oppo_serve_winner_bh + oppo_serve_in_bh) > 0:
        x = x.replace('"pl_return_bh_error"', '{:.0%}'.format(
            pl_return_bh_error / (oppo_serve_ace_bh + oppo_serve_winner_bh + oppo_serve_in_bh)))
        x = x.replace('"oppo_serve_ace_bh"', '{:.0%}'.format(
            oppo_serve_ace_bh / (oppo_serve_ace_bh + oppo_serve_winner_bh + oppo_serve_in_bh)))
    else:
        x = x.replace('"pl_return_bh_error"', '')
        x = x.replace('"oppo_serve_ace_bh"', '')

    if (oppo_serve_ace_body + oppo_serve_winner_body + oppo_serve_in_body) > 0:
        x = x.replace('"pl_return_body_error"', '{:.0%}'.format(
            pl_return_body_error / (oppo_serve_ace_body + oppo_serve_winner_body + oppo_serve_in_body)))
        x = x.replace('"oppo_serve_ace_body"', '{:.0%}'.format(
            oppo_serve_ace_body / (oppo_serve_ace_body + oppo_serve_winner_body + oppo_serve_in_body)))
    else:
        x = x.replace('"pl_return_body_error"', '')
        x = x.replace('"oppo_serve_ace_body"', '')

    out_lines.append(x)

output = open(report_page2_name, 'w')
output.writelines(out_lines)
output.close()
