using System;
using SqlWrapper;
using AngularHomeWork.Models;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Web;
using System.IO;
using System.Data;
using AngularHomeWork.MarshallingObjects;
using System.Net.Http;

namespace AngularHomeWork {

    public static class TheDataStore {
        
        //-----------------------Http Response Message ----------------------------


        public static HttpResponseMessage makeHttpResponseMessage(Response response, RequestObject requestObject, HttpRequestMessage Request, int userId){

            if (!response.isOk) {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, response.message);
            } else {


                DataResponse dataResponse = TheDataStore.getData(requestObject, userId);


                if (!dataResponse.response.isOk) {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, dataResponse.response.message);
                } else {
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, dataResponse.responseObject);
                }
            }
            
        }


        //----------------------Sub request sorter----------------------------

        public static DataResponse getData(SubRequest request, int userId){

            RequestObject requestObject = new RequestObject(request);

            return getData(requestObject, userId);
        }


        public static DataResponse getData(RequestObject request, int userId){
            
            SubRequest[] subRequests = request.subRequests;

            Response response = new Response();

            int numRequests = subRequests.Length;

            SubResponse[] subResponses = new SubResponse[numRequests];


            for (int i = 0; i < numRequests; i++) {

                if (response.isOk) {

                    subResponses[i] = getSubResponse(subRequests[i], response, userId);
                }
            }

            ResponseObject responseObject = new ResponseObject(subResponses);
                                                                 
            DataResponse dataResponse = new DataResponse(responseObject, response);

            return dataResponse;
        }

        private static SubResponse getSubResponse(SubRequest request, Response response, int userId){


            SubResponse subResponse = null;

            switch (request.requestType) {
                case RequestType.classRoom:
                    subResponse = FetchTeacherClassRoom(request, response);
                    break;

                case RequestType.classRoomList:
                    subResponse = FetchClassRoomList(request, response, userId);
                    break;

                case RequestType.assignment:
                    subResponse = FetchTeacherAssignment(request, response);
                    break;

                case RequestType.studentClassRoom:
                    subResponse = FetchStudentClassRoom(request, response, userId);
                    break;
                case RequestType.studentAssignment:
                    subResponse = FetchStudentAssignment(request, response, userId);
                    break;
                case RequestType.subscriptionList:
                    subResponse = FetchSubscriptions(request, response, userId);
                    break;
                case RequestType.outStandingAssignments:
                    subResponse = FetchOutStandingAssignments(request, response, userId);
                    break;
            }

            return subResponse;
        }
        //--------------------Database calls----------------------------

        public static UserTypeResponse getUserType(int userId){

            UserTypeResponse userTypeResponse = new UserTypeResponse();

            userTypeResponse.type = UserType.none;

            SELECT select = new SELECT()
                .col(Users.userType)
                .FROM(Tables.Users)
                .WHERE(new OperatorExpression()
                       .addExpression(Users.userId)
                       .Equals()
                       .addExpression(new IntLiteral(userId))
                      );




            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    userTypeResponse.type = (UserType)reader.GetInt32(Users.userType);
                }
                reader.Close();

            } catch (Exception ex) {

                userTypeResponse.response.setError(ex.ToString());
            }

            conn.Close();


            if(userTypeResponse.type == UserType.none){

                userTypeResponse.response.setError("no user type.");
            }

            return userTypeResponse;

        } 

        public static LoginResponse attemptLogin(string email,string passwordAttempt){

            LoginResponse loginResponse = new LoginResponse();

            string salt = "";
            string saltedHash = "";
            int userType = -1;
            int userId = -1;

            SELECT select = new SELECT()
                .col(LoginDetails.salt)
                .col(LoginDetails.saltedHash)
                .col(Users.userId)
                .col(Users.userType)
                .FROMJOIN(Tables.LoginDetails, Tables.Users, new OperatorExpression().addExpression(LoginDetails.userId).Equals().addExpression(Users.userId))
                .WHERE(new OperatorExpression()
                       .addExpression(LoginDetails.emailAddress)
                       .Equals()
                       .addExpression(new StringLiteral(email))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    salt = reader.GetString(LoginDetails.salt);
                    saltedHash = reader.GetString(LoginDetails.saltedHash);
                    userId = reader.GetInt32(Users.userId);
                    userType = reader.GetInt32(Users.userType);
                }
                reader.Close();

            } catch (Exception ex) {

                loginResponse.response.setError(ex.ToString());
            }

            conn.Close();


            loginResponse.userId = userId;
            loginResponse.userType = (UserType)userType;

            bool passwordIsCorrect = SaltedHash.isPasswordCorrect(passwordAttempt, salt, saltedHash);

            if(passwordIsCorrect){

                loginResponse.success = true;

            }else{
                loginResponse.success = false;
            }

            return loginResponse;

        }




        public static UserResponse registerNewUser(RegisterUserCO cO){
            

            UserResponse userResponse = new UserResponse();


            userResponse.user = new User();

            bool emailAvailable = checkEmail(cO.emailAddress);

            if(!emailAvailable){
                userResponse.response.setError("Email address already in use.");
            }else{

                addUser(userResponse, cO);

                if(!userResponse.response.isOk){
                    return userResponse;
                }else{

                    addLoginDetails(userResponse, cO);


                }
            }

            return userResponse;
        }





        private static bool checkEmail(string email){


            string emailAddress = null;


            SELECT select = new SELECT()
                .col(LoginDetails.emailAddress)
                .FROM(Tables.LoginDetails)
                .WHERE(new OperatorExpression()
                       .addExpression(LoginDetails.emailAddress)
                       .Equals()
                       .addExpression(new StringLiteral(email))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    emailAddress = reader.GetString(LoginDetails.emailAddress);

                }
                reader.Close();

            } catch (Exception ex) {


            }

            conn.Close();


            if(emailAddress == null){
                return true;
            }else{
                return false;
            }

        }


        private static void addUser(UserResponse userResponse, RegisterUserCO cO){

            int newUserId = -1;

            INSERTINTO insert = new INSERTINTO(Tables.Users)
                .ValuePair(Users.userName, new StringLiteral(cO.name))
                .ValuePair(Users.userType, new IntLiteral((int)cO.type))
                .ValuePair(Users.schoolId, new IntLiteral(cO.schoolId));

            //Debug Code------------------------------------------------

            Console.WriteLine(insert.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);



            try {
                conn.Open();
                command.ExecuteNonQuery();
                newUserId = (int)command.LastInsertedId;

            } catch (Exception ex) {

                userResponse.response.setError(ex.ToString());
            }

            conn.Close();

            if(newUserId == -1){

                userResponse.response.setError("User registration failed");

            }else{

                userResponse.userId = newUserId;
            }




        }

        private static void addLoginDetails(UserResponse userResponse, RegisterUserCO cO){


            SaltedHash saltedHash = new SaltedHash(cO.password);


            INSERTINTO insert = new INSERTINTO(Tables.LoginDetails)
                .ValuePair(LoginDetails.userId, new IntLiteral(userResponse.userId))
                .ValuePair(LoginDetails.emailAddress, new StringLiteral(cO.emailAddress))
                .ValuePair(LoginDetails.userPassword, new StringLiteral(cO.password))
                .ValuePair(LoginDetails.salt, new StringLiteral(saltedHash.salt))
                .ValuePair(LoginDetails.saltedHash, new StringLiteral(saltedHash.saltedHash))
                ;

            //Debug Code------------------------------------------------

            Console.WriteLine(insert.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);



            try {
                conn.Open();
                command.ExecuteNonQuery();


            } catch (Exception ex) {

                userResponse.response.setError(ex.ToString());
            }

            conn.Close();

        }



        public static UserResponse FetchStudent(int studentId){

            UserResponse userResponse = new UserResponse();

            string[] subscriptions = getStudentSubs(userResponse.response, studentId);

            Student student = new Student(subscriptions);

            userResponse.user = student;

            getUserData(userResponse, studentId);

            return userResponse;

        }



        public static UserResponse FetchTeacher(int teacherId) {
            UserResponse userResponse = new UserResponse();

            // Collection<ClassRoomListItem> classRooms = makeClassRoomListItems(userResponse.response, teacherId);

            Collection<string> classRoomNames = getClassRoomNames(userResponse.response, teacherId);

            string[] arrayOfCRNames = new string[classRoomNames.Count];

            classRoomNames.CopyTo(arrayOfCRNames, 0);


            Teacher teacher = new Teacher(arrayOfCRNames);

            userResponse.user = teacher;

            getUserData(userResponse, teacherId);

            return userResponse;



        }

        private static void getUserData(UserResponse userResponse, int userId){

            SELECT select = new SELECT()
                .star()
                .FROM(Tables.Users)
                .WHERE(new OperatorExpression()
                       .addExpression(Users.userId)
                       .Equals()
                       .addExpression(new IntLiteral(userId))
                      );
            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    userResponse.user.name = reader.GetString(Users.userName);
                    userResponse.user.schoolId = reader.GetInt32(Users.schoolId);

                }
                reader.Close();

            } catch (Exception ex) {

                userResponse.response.setError(ex.ToString());
            }

            conn.Close();

        }


        private static string[] getStudentSubs(Response response, int studentId){


            Collection<string> subs = new Collection<string>();


            SELECT select = new SELECT()
                .col(Subscriptions.classRoomName)
                .FROM(Tables.Subscriptions)
                .WHERE(new OperatorExpression()
                       .addExpression(Subscriptions.studentId)
                       .Equals()
                       .addExpression(new IntLiteral(studentId))
                      );


            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    subs.Add(reader.GetString(Subscriptions.classRoomName));

                }
                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();


            string[] arrayOfSubs = new string[subs.Count];

            subs.CopyTo(arrayOfSubs, 0);

            return arrayOfSubs;

        }



        private static Collection<string> getClassRoomNames(Response response, int teacherId){

            Collection<string> classRoomNames = new Collection<string>();


            SELECT select = new SELECT()
                .col(ClassRooms.classRoomName)
                .FROM(Tables.ClassRooms)
                .WHERE(new OperatorExpression()
                       .addExpression(ClassRooms.teacherId)
                       .Equals()
                       .addExpression(new IntLiteral(teacherId))
                       .AND()
                       .addExpression(ClassRooms.isClosed)
                       .Equals()
                       .addExpression(new IntLiteral(0))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    classRoomNames.Add(reader.GetString(ClassRooms.classRoomName));

                }
                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();

            return classRoomNames;

        }
       

        private static Collection<ClassRoomListItem> makeClassRoomListItems(Response response, int teacherId) {

            Collection<ClassRoomListItem> classRooms = new Collection<ClassRoomListItem>();

            SELECT select = new SELECT()
                .col(ClassRooms.classRoomName)
                .col(new MATHS(EMathsType.MIN, Assignments.dueDate))
                .FROMJOINLeft(Tables.ClassRooms,
                              Tables.Assignments,
                              new OperatorExpression()
                              .addExpression(ClassRooms.classRoomName)
                              .Equals()
                              .addExpression(Assignments.classRoomName))
                .WHERE(new OperatorExpression()
                       .addExpression(ClassRooms.teacherId)
                       .Equals()
                       .addExpression(new IntLiteral(teacherId))
                       .AND()
                       .addExpression(ClassRooms.isClosed)
                       .Equals()
                       .addExpression(new IntLiteral(0))
                      )
                .GROUPBY(ClassRooms.classRoomName);

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    string date = "";
                    string name = "";


                    var ordinal = reader.GetOrdinal("MIN( Assignments.dueDate)");

                    if (reader.IsDBNull(ordinal)) {

                        date = "No assignment due";
                    } else {
                        date = reader.GetDateTime("MIN( Assignments.dueDate)").ToString("d");
                    }


                    name = reader.GetString(ClassRooms.classRoomName);




                    classRooms.Add(new ClassRoomListItem(name, date));

                }
                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();

            return classRooms;

        }

        //public static Response createClassRoom(string name, int teacherId){
            
        //}


        private static SubResponse FetchSubscriptions(SubRequest request, Response response, int userId){

            SubResponse subResponse = new SubResponse();

            string[] subs = getStudentSubs(response, userId);

            subResponse.requestType = RequestType.subscriptionList;

            subResponse.modelObject = subs;

            return subResponse;

        }

        private static SubResponse FetchOutStandingAssignments(SubRequest request, Response response, int userId){
            SubResponse subResponse = new SubResponse();

            Collection<AssignmentListItem> assignments = new Collection<AssignmentListItem>();

            SELECT select = new SELECT()
                .col(Assignments.assignmentName)
                .col(Assignments.dueDate)
                .col(Assignments.assignmentId)
                .col(AssignmentCompletions.completionDate)
                .FROM(
                    new JOIN(EJoinType.Join, Tables.Assignments, Tables.Subscriptions,
                             new OperatorExpression()
                             .addExpression(Assignments.classRoomName)
                             .Equals()
                             .addExpression(Subscriptions.classRoomName)
                             .AND()
                             .addExpression(Subscriptions.studentId)
                             .Equals()
                             .addExpression(new IntLiteral(userId))
                 ),
                    new JOIN(EJoinType.Left, null, Tables.AssignmentCompletions,
                             new OperatorExpression()
                             .addExpression(AssignmentCompletions.assignmentId)
                             .Equals()
                             .addExpression(Assignments.assignmentId)
                             .AND()
                             .addExpression(AssignmentCompletions.studentId)
                             .Equals()
                             .addExpression(Subscriptions.studentId)

                            )
                )
                .WHERE(new OperatorExpression()
                       .addExpression(Assignments.isClosed)
                       .Equals()
                       .addExpression(new IntLiteral(0))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                   
                    var ordinal = reader.GetOrdinal(AssignmentCompletions.completionDate);

                    if (reader.IsDBNull(ordinal)) {

                        AssignmentListItem assignment = new AssignmentListItem();

                        assignment.dueDate = Convert.ToDateTime(reader[Assignments.dueDate]).ToString("d");
                        assignment.id = reader.GetInt32(Assignments.assignmentId);
                        assignment.title = reader.GetString(Assignments.assignmentName);


                        assignments.Add(assignment);
                    }

                }
                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();

            AssignmentListItem[] arrayOfALs = new AssignmentListItem[assignments.Count];

            assignments.CopyTo(arrayOfALs, 0);

            subResponse.requestType = RequestType.outStandingAssignments;

            subResponse.modelObject = arrayOfALs;

            return subResponse;


        }

        private static SubResponse FetchClassRoomList(SubRequest request,Response response, int teacherId){

            SubResponse subResponse = new SubResponse();

            Collection<string> classRoomList = getClassRoomNames(response, teacherId);

            string[] classRoomListArray = new string[classRoomList.Count];

            for (int i = 0; i < classRoomList.Count; i++){

                classRoomListArray[i] = classRoomList[i];
            }

            subResponse.requestType = RequestType.classRoomList;

            subResponse.modelObject = classRoomListArray;

            return subResponse;

        }

        private static SubResponse FetchStudentClassRoom(SubRequest request, Response response, int userId){
            
            SubResponse subResponse = new SubResponse();

            string name = request.name;

            StudentAssignmentListItem[] assignmentListItems = getStudentAssignmentListItems(response, name, userId);

            subResponse.modelObject = new StudentClassRoom(name, assignmentListItems);

            subResponse.requestType = RequestType.studentClassRoom;

            return subResponse;
        }




        private static SubResponse FetchTeacherClassRoom(SubRequest request, Response response){

            SubResponse subResponse = new SubResponse();


            string name = request.name;

            TeacherAssignmentListItem[] assignmentListItems = getTeacherAssignmentListItems(response, name);

            string[] students = getClassRoomStudents(response, name);

            subResponse.modelObject = new TeacherClassRoom(name, assignmentListItems, students);

            subResponse.requestType = RequestType.classRoom;

            return subResponse;

        }

        private static SubResponse FetchTeacherAssignment(SubRequest request, Response response){
            SubResponse subResponse = new SubResponse();

            int id = request.id;

            Assignment assignment = FetchAssignment(id, response);


            subResponse.modelObject = assignment;

            subResponse.requestType = RequestType.assignment;

            return subResponse;

        }

        private static SubResponse FetchStudentAssignment(SubRequest request, Response response, int userId){

            SubResponse subResponse = new SubResponse();

            int id = request.id;

            Assignment assignment = FetchAssignment(id, response);

            bool markedDone = isAssignmentMarkedDone(id, userId, response);

            StudentAssignment studentAssignment = new StudentAssignment(assignment, markedDone);

            subResponse.modelObject = studentAssignment;

            subResponse.requestType = RequestType.studentAssignment;

            return subResponse;


        }

        private static bool isAssignmentMarkedDone(int assignmentId, int userId, Response response){

            SELECT select = new SELECT()
                .star()
                .FROM(Tables.AssignmentCompletions)
                .WHERE(new OperatorExpression()
                       .addExpression(AssignmentCompletions.studentId)
                       .Equals()
                       .addExpression(new IntLiteral(userId))
                       .AND()
                       .addExpression(AssignmentCompletions.assignmentId)
                       .Equals()
                       .addExpression(new IntLiteral(assignmentId))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            int markedDone = -1;

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    markedDone = reader.GetInt32(AssignmentCompletions.assignmentId);
                }
                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();

            if(markedDone == -1){
                return false;
            }else{
                return true;
            }
        }




        private static Assignment FetchAssignment(int id, Response response) {



            Assignment assignment = new Assignment();

            SELECT select = new SELECT()
                .col(Assignments.assignmentName)
                .col(Assignments.classRoomName)
                .col(Assignments.dueDate)
                .col(Assignments.description)
                .FROM(Tables.Assignments)
                .WHERE(new OperatorExpression()
                       .addExpression(Assignments.assignmentId)
                       .Equals()
                       .addExpression(new IntLiteral(id))
                       .AND()
                       .addExpression(Assignments.isClosed)
                       .Equals()
                       .addExpression(new IntLiteral(0))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    assignment.id = id;
                    assignment.name = reader.GetString(Assignments.assignmentName);
                    assignment.classRoomName = reader.GetString(Assignments.classRoomName);
                    assignment.dueDate = Convert.ToDateTime(reader[Assignments.dueDate]).ToString("d");
                    assignment.description = reader.GetString(Assignments.description);

                }
                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();


            return assignment;

        }

        private static string[] getClassRoomStudents(Response response,string classRoomName){

            Collection<string> students = new Collection<string>();

            SELECT select = new SELECT()
                .col(Users.userName)
                .FROMJOIN(Tables.Subscriptions, Tables.Users, new OperatorExpression().addExpression(Subscriptions.studentId).Equals().addExpression(Users.userId))
                .WHERE(new OperatorExpression()
                       .addExpression(Subscriptions.classRoomName)
                       .Equals()
                       .addExpression(new StringLiteral(classRoomName))
                      );
            
            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    students.Add(reader.GetString(Users.userName));

                }
                reader.Close();

            } catch (Exception ex) {
                
                response.setError(ex.ToString());
            }

            conn.Close();   

            string[] arrayOfStuds = new string[students.Count];

            students.CopyTo(arrayOfStuds, 0);

            return arrayOfStuds;

        }




        private static StudentAssignmentListItem[] getStudentAssignmentListItems(Response response, string name, int studentId){

            Collection<StudentAssignmentListItem> assignments = new Collection<StudentAssignmentListItem>();

            SELECT select = new SELECT()
                .col(Assignments.assignmentName)
                .col(Assignments.assignmentId)
                .col(Assignments.dueDate)
                .col(AssignmentCompletions.completionDate)
                .FROMJOINLeft(Tables.Assignments, Tables.AssignmentCompletions, new OperatorExpression()
                              .addExpression(Assignments.assignmentId)
                              .Equals()
                              .addExpression(AssignmentCompletions.assignmentId)
                              .AND()
                              .addExpression(AssignmentCompletions.studentId)
                              .Equals()
                              .addExpression(new IntLiteral(studentId))
                             )
                .WHERE(new OperatorExpression()
                       .addExpression(Assignments.classRoomName)
                       .Equals()
                       .addExpression(new StringLiteral(name))
                       .AND()
                       .addExpression(Assignments.isClosed)
                       .Equals()
                       .addExpression(new IntLiteral(0))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {
                    bool isDone = true;

                    var ordinal = reader.GetOrdinal(AssignmentCompletions.completionDate);

                    if (reader.IsDBNull(ordinal)) {

                        isDone = false;
                    }

                    assignments.Add(new StudentAssignmentListItem(
                        reader.GetString(Assignments.assignmentName),
                        reader.GetInt32(Assignments.assignmentId),
                        Convert.ToDateTime(reader[Assignments.dueDate]).ToString("d"),
                        isDone
                    ));


                }
                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();


            StudentAssignmentListItem[] arrayOfALs = new StudentAssignmentListItem[assignments.Count];

            assignments.CopyTo(arrayOfALs, 0);


            return arrayOfALs;

        }




        private static TeacherAssignmentListItem[] getTeacherAssignmentListItems(Response response, string name){

            Collection<TeacherAssignmentListItem> assignments = new Collection<TeacherAssignmentListItem>();

            SELECT select = new SELECT()
                .col(Assignments.assignmentId)
                .col(Assignments.assignmentName)
                .col(Assignments.dueDate)
                .FROM(Tables.Assignments)
                .WHERE(new OperatorExpression()
                       .addExpression(Assignments.classRoomName)
                       .Equals()
                       .addExpression(new StringLiteral(name))
                       .AND()
                       .addExpression(Assignments.isClosed)
                       .Equals()
                       .addExpression(new IntLiteral(0))
                      );
                                
            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    TeacherAssignmentListItem assignmentListItem = new TeacherAssignmentListItem();


                    assignmentListItem.id = reader.GetInt32(Assignments.assignmentId);

                    assignmentListItem.title = reader.GetString(Assignments.assignmentName);


                    assignmentListItem.dueDate =   Convert.ToDateTime(reader[Assignments.dueDate]).ToString("d");
                   

                    assignments.Add(assignmentListItem);

                }
                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();


            TeacherAssignmentListItem[] arrayOfALs = new TeacherAssignmentListItem[assignments.Count];

            assignments.CopyTo(arrayOfALs, 0);


            return arrayOfALs;
        }


        public static Response createClassRoom(int teacherId, string classRoomName){
            Response response = new Response();

            INSERTINTO insert = new INSERTINTO(Tables.ClassRooms)
                .ValuePair(ClassRooms.classRoomName, new StringLiteral(classRoomName))
                .ValuePair(ClassRooms.teacherId, new IntLiteral(teacherId));

            //Debug Code------------------------------------------------

            Console.WriteLine(insert.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);



            try {
                conn.Open();
                command.ExecuteNonQuery();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();



            return response;
        }

        public static Response createAssignment(string newName, string classRoomName, DateTime newDueDate, string newDescription){
            Response response = new Response();


            INSERTINTO insert = new INSERTINTO(Tables.Assignments)
                .ValuePair(Assignments.assignmentName, new StringLiteral(newName))
                .ValuePair(Assignments.classRoomName, new StringLiteral(classRoomName))
                .ValuePair(Assignments.dueDate, new DateLiteral(newDueDate))
                .ValuePair(Assignments.description, new StringLiteral(newDescription));

            //Debug Code------------------------------------------------

            Console.WriteLine(insert.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);



            try {
                conn.Open();
                command.ExecuteNonQuery();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();

            return response;
        }


        public static Response editAssignment(int id, string newName, DateTime newDueDate, string newDescription, int newArchiveStatus){

            Response response = new Response();




            UPDATE update = new UPDATE(Tables.Assignments)
                .addValuePair(Assignments.assignmentName, new StringLiteral(newName))
                .addValuePair(Assignments.dueDate, new DateLiteral(newDueDate))
                .addValuePair(Assignments.description, new StringLiteral(newDescription))
                .addValuePair(Assignments.isClosed, new IntLiteral(newArchiveStatus))
                .WHEREEQUALS(Assignments.assignmentId, new IntLiteral(id));

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();


            return response;

        }


        public static Response changeClassRoomArchiveStatus(string classRoomName, int archiveStatus){
            Response response = new Response();

            UPDATE update = new UPDATE(Tables.ClassRooms)
                .addValuePair(ClassRooms.isClosed, new IntLiteral(archiveStatus))
                .WHEREEQUALS(ClassRooms.classRoomName, new StringLiteral(classRoomName));

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();


            return changeClassRoomAssignmentsArchiveStatus(response, classRoomName, archiveStatus); 

        }

        private static Response changeClassRoomAssignmentsArchiveStatus(Response response, string classRoomName, int archiveStatus){

            UPDATE update = new UPDATE(Tables.Assignments)
                .addValuePair(Assignments.isClosed, new IntLiteral(archiveStatus))
                .WHEREEQUALS(Assignments.classRoomName, new StringLiteral(classRoomName));

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();


            return response;

        }

        public static Response changeUserArchiveStatus(int userId, UserType type, int archiveStatus){
            Response response = new Response();

            UPDATE update = new UPDATE(Tables.Users)
                .addValuePair(Users.isClosed, new IntLiteral(archiveStatus))
                .WHEREEQUALS(Users.userId, new IntLiteral(userId));

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();


            if (type == UserType.teacher) {
                return changeTeacherClassRoomsArchiveStatus(response, userId, archiveStatus);
            }else{
                return response;
            }



        }

        private static Response changeTeacherClassRoomsArchiveStatus(Response response, int userId, int archiveStatus ){

            UPDATE update = new UPDATE(Tables.ClassRooms)
                .addValuePair(ClassRooms.isClosed, new IntLiteral(archiveStatus))
                .WHEREEQUALS(ClassRooms.teacherId, new IntLiteral(userId));

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();

            return changeTeacherAssignmentsArchiveStatus(response, userId, archiveStatus);

        }

        private static Response changeTeacherAssignmentsArchiveStatus(Response response, int userId, int archiveStatus){
            
            UPDATE update = new UPDATE(Tables.Assignments)
                .JOIN(Tables.ClassRooms, new OperatorExpression()
                      .addExpression(Assignments.classRoomName)
                      .Equals()
                      .addExpression(ClassRooms.classRoomName)
                     )
                .addValuePair(Assignments.isClosed, new IntLiteral(archiveStatus))
                .WHEREEQUALS(ClassRooms.teacherId, new IntLiteral(userId));

            //Debug Code------------------------------------------------

            Console.WriteLine(update.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = update.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();

            return response;
                
        }

        public static Response changeDoneState(int assignmentId, bool currentDoneState, int userId){

            Response response = new Response();

            if(currentDoneState == false){

                markAssignmentDone(assignmentId, userId, response);

            }else{

                deleteMarkedDone(assignmentId, userId, response);

            }


            return response;
        }

        private static void markAssignmentDone(int assignmentId, int userId, Response response){

            INSERTINTO insert = new INSERTINTO(Tables.AssignmentCompletions)
                .ValuePair(AssignmentCompletions.assignmentId, new IntLiteral(assignmentId))
                .ValuePair(AssignmentCompletions.studentId, new IntLiteral(userId))
                .ValuePair(AssignmentCompletions.completionDate, new DateLiteral(DateTime.Now))
                ;

            //Debug Code------------------------------------------------

            Console.WriteLine(insert.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);



            try {
                conn.Open();
                command.ExecuteNonQuery();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();

        }


        private static void deleteMarkedDone(int assignmentId, int userId, Response response){

            DELETE delete = new DELETE().FROM(Tables.AssignmentCompletions).WHERE(new OperatorExpression()
                                                                                  .addExpression(AssignmentCompletions.assignmentId)
                                                                                  .Equals()
                                                                                  .addExpression(new IntLiteral(assignmentId))
                                                                                  .AND()
                                                                                  .addExpression(AssignmentCompletions.studentId)
                                                                                  .Equals()
                                                                                  .addExpression(new IntLiteral(userId))
                                                                                 );

            //Debug Code------------------------------------------------

            Console.WriteLine(delete.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = delete.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

        }



        public static Response unsubscribeToClassRoom(int userId, string classRoomName){

            Response response = new Response();

            DELETE delete = new DELETE().FROM(Tables.Subscriptions).WHERE(new OperatorExpression()
                                                                                  .addExpression(Subscriptions.studentId)
                                                                                  .Equals()
                                                                                  .addExpression(new IntLiteral(userId))
                                                                                  .AND()
                                                                                  .addExpression(Subscriptions.classRoomName)
                                                                                  .Equals()
                                                                                  .addExpression(new StringLiteral(classRoomName))
                                                                                 );

            //Debug Code------------------------------------------------

            Console.WriteLine(delete.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------


            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = delete.makeMySqlCommand(conn, ERenderType.Paramed);

            try {
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }



            return response;
        }




        public static Response subscribeToClassRoom(int userId, string classRoomName){

            Response response = new Response();

            bool exists = doesRoomExist(classRoomName, response);

            if (exists) {
                
                    makeSubscription(userId, classRoomName, response);

            }else{

                response.setError(classRoomName + " doesn't exist");
            }

            return response;
        }


        private static bool doesRoomExist(string classRoomName, Response response){

            bool exists = false;

            string name = null;


            SELECT select = new SELECT()
                .col(ClassRooms.classRoomName)
                .FROM(Tables.ClassRooms)
                .WHERE(new OperatorExpression()
                       .addExpression(ClassRooms.classRoomName)
                       .Equals()
                       .addExpression(new StringLiteral(classRoomName))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    name = reader.GetString(ClassRooms.classRoomName);
                }
                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();

            if(name == classRoomName){

                exists = true;
            }

            return exists;
        }


        private static bool isUserAlreadySubscribed(int userId, string classRoomName, Response response){

            bool alreadySubbed = false;

            string name = "";

            SELECT select = new SELECT()
                .col(Subscriptions.classRoomName)
                .FROM(Tables.Subscriptions)
                .WHERE(new OperatorExpression()
                       .addExpression(Subscriptions.classRoomName)
                       .Equals()
                       .addExpression(new StringLiteral(classRoomName))
                       .AND()
                       .addExpression(Subscriptions.studentId)
                       .Equals()
                       .addExpression(new IntLiteral(userId))
                      );

            //Debug Code------------------------------------------------

            Console.WriteLine(select.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = select.makeMySqlCommand(conn, ERenderType.Paramed);

            try {

                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read()) {

                    name = reader.GetString(ClassRooms.classRoomName);
                }
                reader.Close();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }


            conn.Close();

            if (name == classRoomName) {

                alreadySubbed = true;
            }

            return alreadySubbed;
        }


        private static void makeSubscription(int userId, string classRoomName, Response response){

            INSERTINTO insert = new INSERTINTO(Tables.Subscriptions)
                .ValuePair(Subscriptions.classRoomName, new StringLiteral(classRoomName))
                .ValuePair(Subscriptions.studentId, new IntLiteral(userId))
                ;

            //Debug Code------------------------------------------------

            Console.WriteLine(insert.render(ERenderType.NonParamed));
            //Debug Code------------------------------------------------

            MySqlConnection conn = new MySqlConnection(DataKeys.dataBaseConnectionString);

            MySqlCommand command = insert.makeMySqlCommand(conn, ERenderType.Paramed);



            try {
                conn.Open();
                command.ExecuteNonQuery();

            } catch (Exception ex) {

                response.setError(ex.ToString());
            }

            conn.Close();
        }
    }
}
