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
namespace AngularHomeWork {

    public static class TheDataStore {

        //----------------------Sub request sorter----------------------------

        public static DataResponse getData(SubRequest request){


            DataResponse dataResponse = null;

            switch (request.requestType){
                case RequestType.assignmentsForClassRoom:
                    dataResponse = FetchClassRoom(request);
                    break;
            }


            return dataResponse;
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
                       .addExpression(new IntLiteral(teacherId)));

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

        public static ClassRoomResponse FetchClassRoom(SubRequest request){

            string name = request.name;

            ClassRoomResponse classRoomResponse = new ClassRoomResponse();



            Collection<AssignmentListItem> assignments = getAssignmentListItems(classRoomResponse.response, name);

            AssignmentListItem[] arrayOfALs = new AssignmentListItem[assignments.Count];

            assignments.CopyTo(arrayOfALs, 0);

            classRoomResponse.classRoom = new ClassRoom(name, arrayOfALs);

            return classRoomResponse;

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
                       .addExpression(new StringLiteral(name)));
                                
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

            return response;
        }

    }
}
