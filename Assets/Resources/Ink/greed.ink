// Mixed event. Gain a bunch of Scrips at the cost of a random negative trait.
VAR player = ""
VAR ally = ""
VAR scrips_gain = 100

An uncharacteristic jingling sound raises the hairs on your neck. # narrato
It's Chester! # narrator #enter:player # enter:enemy
Oh-ho, it's {player}. And {ally}'s along for the car-ride too! #enemy
What're you looking for? Weather-glasses? Boom-sticks? Or... # enemy
... # enemy
Scrips? Good-old scrips? # enemy
I'll consider the scrips.
Excellence! You know the rules, {player} - just a tad-bit penalty for some freshly minted Scrips! # enemy
* [Take the Offer - gain {scrips_gain} Scrips and a Negative Quirk]You nod and offer your hand to Chester. # narrator
Chester grins and slaps {scrips_gain} scrips into your hand. # narrator
Your hand suddenly burns in pain. # narrator
Gained NEGATIVE QUIRK. # narrator
An excellent-deal! I'll see you again, in, say, two weeks? Three, perhaps? # enemy
Ideally, never. But your line of work doesn't exactly afford such luxuries. # narrator
* [Back off] Not today.
Oh well. Your loss, {player}! # enemy
Don't worry. If you're ever in need, I'll come by and find you! # enemy