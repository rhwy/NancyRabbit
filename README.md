NancyRabbit
===========

This a command line self host with auto reload feature for the Nancy web framework

The target is to get a simple host to quickly develop web clients with the support of Nancy framework 
from the server/api side.
This should be considered as something like `shotgun` in the Ruby/Sinatra world.

When you work on your web interface, it is usuful to have a server running with files or mockup apis 
in order to really respond to your ajax calls.
With a scripting language loaded page by page it is just trivial (yes, I'm looking at you old php boy), 
with a scripting language running an application that needs a bootstrap initialization, routing management 
and so on, it is more complicated (not really, but you need a tool, like Shotgun in Sinatra world),
and in a compiled world, it can be painful...

So that's the targets of Nancy Rabbit:

* write a minimal Nancy module class in your working folder
* Run `rabbit.exe` on this folder
* Browse to it
* Add a new route to your module
* just Reload, it works, your changes here ;-)

**NOTE**:

  This project in it's current state is still a POC (but a working one ;-). Due to the good feedbacks, I am planning making it cleaner and have a lot of stuff in mind.
  please, put your ideas and feedbacks in the issues section in this github if you have any!

Thanks to [@TheCodeJunkie](https://github.com/thecodejunkie), we have now a small demo of .net web dev with NancyRabbit:

<iframe src="http://player.vimeo.com/video/53459633?badge=0" width="500" height="281" frameborder="0" webkitAllowFullScreen mozallowfullscreen allowFullScreen></iframe> <p><a href="http://vimeo.com/53459633">Demo of NancyRabbit</a> from <a href="http://vimeo.com/user7634348">Nancy</a> on <a href="http://vimeo.com">Vimeo</a>.</p>
