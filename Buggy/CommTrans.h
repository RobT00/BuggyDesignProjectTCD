#pragma once

#include "HashMap.h"
#include "Functions.h"

#include <Arduino.h>

class CommTrans{
  public:
    CommTrans() {}
    void init();
    void writeXbee(String command);
    void processCommand(char c);
    void addHandler(String command, VoidFunction handler);

  private:
    String message;
    HashMap handlers;
    static const short my_ID = 1;
};

