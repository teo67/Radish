namespace Tools {
    class RadishException : Exception{
        public int? Row { get; private set; }
        public int? Col { get; private set; }
        public string RMessage { get; private set; }
        public RadishException(string message, int? row = null, int? col = null) {
            this.RMessage = message;
            this.Row = row;
            this.Col = col;
        }
        public RadishException Append(string message, int? row = null, int? col = null) {
            this.RMessage += message;
            if(Row == null && row != null) {
                Row = row;
            }
            if(Col == null && col != null) {
                Col = col;
            }
            return this;
        }
    }
}