// Beneficial event which transforms a card in your deck; player chooses to transform it to 1 of 3 random rares at the cost of Scrips.
// If Ai is an ally, increases to 1 of 5 random rares.
// If Deckard is an ally, reduce Scrip cost.
// Event is exclusive to the Downwell.
VAR player = ""
VAR ally = ""

As you and {ally} comb your way through the back alleys of the Downwell, the ever-present humming suddenly gives way to the clear sound of clanging on metal.
{(ally == "Ai" or player == "Ai"): It's the Alternator!} # ally
{player == "Deckard": Well, look who it is - {player}! Haven't seen you in a while. | You're clearly not from around here.} # enemy
{ally == "Ai"} But you are! # enemy