class CommTrans{
  public:
    CommTrans();
    void writeXbee(String command);
    void processCommand(char c);
    void init();

  private:
     String command;
     short send_ID;
     short revi_ID;
}

