#pragma once

#include "Buggy.h"
#include "CommTrans.h"
#include "HashMap.h"
#include "MotorControls.h"




class UltraSonic{
  
  UltraSonic(); //Constructor
  unsigned long UltraLoop();  // Loop to read distance, etc.
};