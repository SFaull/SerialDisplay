// commands.h

#ifndef _COMMANDS_h
#define _COMMANDS_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "arduino.h"
#else
	#include "WProgram.h"
#endif


class Commands
{
 protected:


 public:
    void init(void);
    void process(void);
};

#endif