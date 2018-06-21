using System;
namespace Pupil {
    //--------------------RESPONSES-------------------------

    public enum ResponseState {
        ok = 0,
        error = 1,
        bug = 2,

    }

    public enum SubResponseType {
        none = 0,
    }

    public class Response {
        public ResponseState state = ResponseState.ok;
        SubResponseType subState = SubResponseType.none;
        public string message = "";

        public Response() {

        }

        public Response(ResponseState state, string message) {

            this.state = state;
            this.message = message;
        }

        public Response(ResponseState state, SubResponseType subState, string message) {

            this.state = state;
            this.subState = subState;
            this.message = message;
        }

        public bool isOk {

            get { return this.state == ResponseState.ok; }

        }

        public void setError(string message) {

            this.state = ResponseState.error;
            this.message = message;

        }
    }

}
