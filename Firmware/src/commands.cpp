
#include <SerialCommand.h>
#include <TFT_eSPI.h>       // Hardware-specific library

#include "commands.h"
#include "image.h"

#define DISPLAY_WIDTH   240
#define DISPLAY_HEIGHT  240
#define DISPLAY_PIXELS  (DISPLAY_WIDTH * DISPLAY_HEIGHT)

#define MAX_DISPLAY_BLOCK_SIZE     110
#define MAX_DISPLAY_BLOCK_PIXELS  (MAX_DISPLAY_BLOCK_SIZE * MAX_DISPLAY_BLOCK_SIZE)

#define SERIAL_READ_BLOCK_SIZE      255

typedef union
{
	char bytes[MAX_DISPLAY_BLOCK_PIXELS*2];
	uint16_t pixels[MAX_DISPLAY_BLOCK_PIXELS] = {0};
} display_store_t;

display_store_t displayStore;


SerialCommand sCmd;     // The demo SerialCommand object
TFT_eSPI tft = TFT_eSPI();  // Invoke custom library

static void idn(void);
static void display_set_rotation(void);
static void display_block(void);
static void speed_test(void);
static void print_buffer(void);
static void unrecognized(const char *command);

void Commands::init()
{
  tft.init();
  tft.setSwapBytes(true); // Swap the byte order for pushImage() - corrects endianness
  tft.fillScreen(TFT_BLACK);
  //tft.pushImage(179,0,DISPLAY_BLOCK_SIZE,DISPLAY_BLOCK_SIZE,image);

  sCmd.addCommand("*IDN?", idn);        // Echos the string argument back
  sCmd.addCommand("TILE", display_block);        // Echos the string argument back
  sCmd.addCommand("TEST", speed_test);
  sCmd.addCommand("BUFFER?", print_buffer);        // Echos the string argument back
  sCmd.addCommand("ROTATION", display_set_rotation);       
  sCmd.setDefaultHandler(unrecognized);      // Handler for command that isn't matched  (says "What?")
}

void Commands::process()
{
    sCmd.readSerial();     // We don't do much, just process serial commands
}

static void idn() {
    SerialUSB.println("Xiao");
}

static void display_set_rotation() {
    char *arg; // expect 1 arguments
    arg = sCmd.next();
    if(arg == NULL)
    {
        SerialUSB.println("Inavlid Args");
        return; // any missing args and we exit the function
    }


    // convert ASCII parameters to integers
    int rotation = atoi(arg);

    if(rotation < 0 || rotation > 3)
    {
        SerialUSB.println("Inavlid Args");
        return; // any missing args and we exit the function
    }

    tft.setRotation(rotation);
}


static void display_block() {
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
    if(w * h > MAX_DISPLAY_BLOCK_PIXELS)
    {
        //SerialUSB.println("Too large");
        return;
    }

    long bytesToRead = w * h * sizeof(uint16_t);
    char * ptr = displayStore.bytes;

    // now lets read the raw data into the buffer
    while(bytesToRead > 0)
    {
        int blockSize = SERIAL_READ_BLOCK_SIZE;
        if(bytesToRead < SERIAL_READ_BLOCK_SIZE)
            blockSize = bytesToRead;

        while(!SerialUSB.available()) {};   // wait until bytes available
        uint8_t bytesReceived = SerialUSB.readBytes(ptr, blockSize);

        if(bytesReceived != blockSize)
        {
            SerialUSB.println("Error receiving image");
            return;
        }

        // increment the buffer and decrement the pixels remaing counter
        ptr+=blockSize;
        bytesToRead-=blockSize;
    }

    //now lets actually display the image
    tft.pushImage(x,y,w,h,displayStore.pixels);
}

static void speed_test() {
    char *arg; // expect 1 arguments

    arg = sCmd.next();  
    if(arg == NULL)
    {
        SerialUSB.println("Inavlid Args");
        return; // any missing args and we exit the function
    }

    // convert ASCII parameters to integers
    long bytesToRead = atol(arg);
    long timeStart = millis();
    char * ptr = displayStore.bytes;
    while(bytesToRead > 0)
    {
        int blockSize = SERIAL_READ_BLOCK_SIZE;
        if(bytesToRead < SERIAL_READ_BLOCK_SIZE)
        {
            blockSize = bytesToRead;
        }

        while(!SerialUSB.available()) {};   // wait until bytes available
        uint16_t bytesReceived = SerialUSB.readBytes(ptr, blockSize);

        if(bytesReceived != blockSize)
        {
            SerialUSB.println("Error receiving image");
            return;
        }
        
        bytesToRead -= blockSize;
        //ptr += blockSize;
    }    

    long timeTaken = millis() - timeStart;
    SerialUSB.println(timeTaken);
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
    uint16_t * ptr = &displayStore.pixels[offset];

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

