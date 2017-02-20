#pragma once

#include "HashMap.h"
#include "Functions.h"

#include <Arduino.h>

class CommTrans {
  public:
    CommTrans() {}
    void init() const;
    void writeXbee(String command) const;
    void processCommand(char c);
    void addHandler(String command, VoidFunction handler);
    void setDefaultHandler(VoidFunction handler);

  private:
    String message;
    HashMap handlers;
    VoidFunction defaultHandler = NULL;
    static const short my_ID = 1;
};
