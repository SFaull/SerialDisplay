#include <Arduino.h>
#include "commands.h"

#define BAUD_RATE 1000000//115200 <-- this doesn't actually matter, the init is a dummy function anyway. It's native USB


/* make sure that User_Setup in eSPI library is configured with the following pins
 * #define TFT_DC   2  // Data Command control pin
 * #define TFT_RST  3  // Reset pin 
 * #define TFT_BL   1  // LED back-light (only for ST7789 with backlight control pin)
 */

Commands cmd;

void setup() {
    SerialUSB.begin(BAUD_RATE);
    cmd.init();
}

void loop() {
  cmd.process();
}

