VAR player = ""
VAR ally = ""

Test story. Main character is {player}. Ally is {ally}.     # narrator
This is a second line in the test story. What happens to the renderer if I end up making this sentence quite long? How about super long? # narrator
I like this story. Third line btw. # narrator
The player thinks...
...and thinks...
...and thinks...
{ally}: I think you should choose option A. # ally
* [Option A]{player} chose option A.
    "The fool chose option A!"  # enemy
* [Option B]You chose option B.
    "The fool chose option B!"  # enemy