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


        public static HttpResponseMessage makeHttpResponseMessage(Response response, RequestObject requestObject, HttpRequestMessage Request){

            if (!response.isOk) {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, response.message);
            } else {


                DataResponse dataResponse = TheDataStore.getData(requestObject);


                if (!dataResponse.response.isOk) {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, dataResponse.response.message);
                } else {
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, dataResponse.responseObject);
                }
            }
            
        }


        //----------------------Sub request sorter----------------------------

        public static DataResponse getData(SubRequest request){

            RequestObject requestObject = new RequestObject(request);

            return getData(requestObject);
        }


        public static DataResponse getData(RequestObject request){
            
            SubRequest[] subRequests = request.subRequests;

            Response response = new Response();

            int numRequests = subRequests.Length;

            SubResponse[] subResponses = new SubResponse[numRequests];


            for (int i = 0; i < numRequests; i++) {

                if (response.isOk) {

                    subResponses[i] = getSubResponse(subRequests[i], response);
                }
            }

            ResponseObject responseObject = new ResponseObject(subResponses);
                                                                 
            DataResponse dataResponse = new DataResponse(responseObject, response);

            return dataResponse;
        }

        private static SubResponse getSubResponse(SubRequest request, Response response){


            SubResponse subResponse = null;

            switch (request.requestType) {
                case RequestType.classRoom:
                    subResponse = FetchClassRoom(request, response);
                    break;

                case RequestType.classRoomList:
                    subResponse = FetchClassRoomList(request, response);
                    break;
            }

            return subResponse;
        }
        //--------------------Database calls----------------------------




        public static UserResponse FetchTeacher(int teacherId) {
            UserResponse userResponse = new UserResponse();

            // Collection<ClassRoomListItem> classRooms = makeClassRoomListItems(userResponse.response, teacherId);

            Collection<string> classRoomNames = getClassRoomNames(userResponse.response, teacherId);

            string[] arrayOfCRNames = new string[classRoomNames.Count];

            classRoomNames.CopyTo(arrayOfCRNames, 0);


            Teacher teacher = new Teacher(teacherId, arrayOfCRNames);

            userResponse.user = teacher;

            SELECT select = new SELECT()
                .star()
                .FROM(Tables.Users)
                .WHERE(new OperatorExpression()
                       .addExpression(Users.userId)
                       .Equals()
                       .addExpression(new IntLiteral(teacherId))
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

                    teacher.name = reader.GetString(Users.userName);
                    teacher.schoolId = reader.GetInt32(Users.schoolId);

                }
                reader.Close();

            } catch (Exception ex) {

                userResponse.response.setError(ex.ToString());
            }

            conn.Close();

            return userResponse;



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

        private static SubResponse FetchClassRoomList(SubRequest request,Response response){

            SubResponse subResponse = new SubResponse();

            int teacherId = request.id;

            Collection<string> classRoomList = getClassRoomNames(response, teacherId);

            string[] classRoomListArray = new string[classRoomList.Count];

            for (int i = 0; i < classRoomList.Count; i++){

                classRoomListArray[i] = classRoomList[i];
            }

            subResponse.requestType = RequestType.classRoomList;

            subResponse.modelObject = classRoomListArray;

            return subResponse;

        }



        private static SubResponse FetchClassRoom(SubRequest request, Response response){

            SubResponse subResponse = new SubResponse();


            string name = request.name;

            Collection<AssignmentListItem> assignments = getAssignmentListItems(response, name);

            AssignmentListItem[] arrayOfALs = new AssignmentListItem[assignments.Count];

            assignments.CopyTo(arrayOfALs, 0);

            subResponse.modelObject = new ClassRoom(name, arrayOfALs);

            subResponse.requestType = RequestType.classRoom;

            return subResponse;

        }

        private static Collection<AssignmentListItem> getAssignmentListItems(Response response, string name){

            Collection<AssignmentListItem> assignments = new Collection<AssignmentListItem>();

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

                    AssignmentListItem assignmentListItem = new AssignmentListItem();


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

            return assignments;
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

    }
}
