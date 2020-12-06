
#include <SerialCommand.h>
#include <TFT_eSPI.h>       // Hardware-specific library

#include "commands.h"
#include "image.h"

#define DISPLAY_WIDTH   240
#define DISPLAY_HEIGHT  240
#define DISPLAY_PIXELS  (DISPLAY_WIDTH * DISPLAY_HEIGHT)

#define DISPLAY_BLOCK_SIZE   60
#define DISPLAY_BLOCK_PIXELS  (DISPLAY_BLOCK_SIZE * DISPLAY_BLOCK_SIZE)

uint16_t display_buffer[DISPLAY_BLOCK_PIXELS] = {0};


SerialCommand sCmd;     // The demo SerialCommand object
TFT_eSPI tft = TFT_eSPI();  // Invoke custom library

static void idn(void);
static void display(void);
static void print_buffer(void);
static void unrecognized(const char *command);

void Commands::init()
{
  tft.init();
  tft.setSwapBytes(true); // Swap the byte order for pushImage() - corrects endianness
  tft.fillScreen(TFT_BLACK);
  //tft.pushImage(179,0,DISPLAY_BLOCK_SIZE,DISPLAY_BLOCK_SIZE,image);

  sCmd.addCommand("*IDN?", idn);        // Echos the string argument back
  sCmd.addCommand("DISPLAY", display);        // Echos the string argument back
  sCmd.addCommand("BUFFER?", print_buffer);        // Echos the string argument back
  sCmd.setDefaultHandler(unrecognized);      // Handler for command that isn't matched  (says "What?")
}

void Commands::process()
{
    sCmd.readSerial();     // We don't do much, just process serial commands
}

static void idn() {
    SerialUSB.println("Xiao");
}


static void display() {
    const uint8_t argc = 4;
    char *arg[argc]; // expect 4 arguments

    // Get the next argument from the SerialCommand object buffer
    for(uint8_t i = 0 ; i < argc; i++)
    {
        arg[i] = sCmd.next();  
        if(arg[i] == NULL)
        {
            SerialUSB.println("Inavlid Args");
            return; // any missing args and we exit the function
        }
    }

    // convert ASCII parameters to integers
    int x = atoi(arg[0]);
    int y = atoi(arg[1]);
    int w = atoi(arg[2]);
    int h = atoi(arg[3]);

    // check args are in valid range
    if(w > DISPLAY_BLOCK_SIZE || h > DISPLAY_BLOCK_SIZE)
    {
        //SerialUSB.println("Too large");
        //return;
    }

    if(((x + w) >= DISPLAY_WIDTH) || ((y+h) >= DISPLAY_WIDTH))
    {
        //SerialUSB.println("Too large");
        //return;
    }
    
    long pixelsRemaining = w * h;

    // workout where in the display buffer we are starting based on the provided offset
    //long offset = (y * DISPLAY_WIDTH) + x;
    // create a pointer to that position in the buffer
    uint16_t * ptr = display_buffer;

    // now lets read the raw data into the buffer
    while(pixelsRemaining > 0)
    {
        // read two bytes at a time
        char pixel[2];
        while(!SerialUSB.available()) {};   // wait until bytes available
        uint8_t bytesReceived = SerialUSB.readBytes(pixel, 2);

        if(bytesReceived != 2)
        {
            SerialUSB.println("Error receiving image");
            return;
        }
        // store this in the uint16_t buffer
        //*ptr = ( (pixel[0] * 0xFF) | (pixel[1] >> 8) );
        
        *ptr = ((pixel[0]<<8)+pixel[1]);
        // increment the buffer and decrement the pixels remaing counter
        ptr++;
        pixelsRemaining--;
    }

    //now lets actually display the image
    tft.pushImage(x,y,DISPLAY_BLOCK_SIZE,DISPLAY_BLOCK_SIZE,display_buffer);

#if 0
    for(long i = 0; i < (w * h); i++)
    {
        SerialUSB.print(display_buffer[i], HEX);
        if((i % DISPLAY_BLOCK_SIZE) == 59)
            SerialUSB.println();
        else
            SerialUSB.print(",");
    }
#endif 
}

static void print_buffer()
{
    const uint8_t argc = 2;
    char *arg[argc]; // expect 2  arguments

    // Get the next argument from the SerialCommand object buffer
    for(uint8_t i = 0 ; i < argc; i++)
    {
        arg[i] = sCmd.next();  
        if(arg[i] == NULL)
        {
            SerialUSB.println("Inavlid Args");
            return; // any missing args and we exit the function
        }
    }

    int offset = atoi(arg[0]);
    int len = atoi(arg[1]);
    uint16_t * ptr = &display_buffer[offset];

    while(len > 0)
    {
        SerialUSB.print(*ptr, HEX);
        ptr++;
        len--;

        if(len == 0)
            SerialUSB.println("");
        else
            SerialUSB.print(", ");
        
    }
}

// This gets set as the default handler, and gets called when no other command matches.
static void unrecognized(const char *command) {
  SerialUSB.println("What?");
}

