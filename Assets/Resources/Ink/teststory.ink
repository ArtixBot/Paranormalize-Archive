VAR player = ""
VAR ally = ""

Test story. Main character is {player}. Ally is {ally}. # narrator
This is a second line in the test story. What happens to the renderer if I end up making this sentence quite long? How about super long? # narrator #enter:player #enter:enemy  
I like this story. Third line btw. # narrator # exit:enemy
The player thinks... #exit:player #enter:enemy
...and thinks...
...and thinks... #enter:player
{ally}: I think you should choose option A. #exit:player #enter:ally
* [Option A]{player} chose option A. #enter:player #exit:ally
    "The fool chose option A!"  # enemy
* [Option B]You chose option B. #enter:player #exit:ally
    "The fool chose option B!"  # enemy #exit:enemy