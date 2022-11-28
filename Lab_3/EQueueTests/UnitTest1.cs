using Xunit;
using Moq;
using System;
using System.Collections.Generic;
namespace UnitTests
{
    public class EqueueTests
    {
        private Equeue equeue;
        private Mock<IAppointment> AppointmentMock = new Mock<IAppointment>();
        private Mock<IAppointment> AppointmentMock1 = new Mock<IAppointment>();
        private Mock<IAppointment> AppointmentMock2 = new Mock<IAppointment>();
        private Mock<IAppointment> AppointmentMock3 = new Mock<IAppointment>();
        public EqueueTests()
        {
            equeue = new Equeue();
        }
        public bool Matches(List<IAppointment> list1, List<IAppointment> list2)
        {
            if (list1.Count != list2.Count) return false;
            for (var i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i]) return false;
            }
            return true;
        }
        public bool Matches(List<ReceptionHours> list1, List<ReceptionHours> list2)
        {
            if (list1.Count != list2.Count) return false;
            for (var i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i]) return false;
            }
            return true;
        }
        [Fact]
        public void successful_adding_one_new_appointment() 
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.dentist;
            AppointmentMock
                .Setup(x => x.GetName())
                .Returns("Vova");
            AppointmentMock
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            //Act
            bool success = equeue.AddAppointment(AppointmentMock.Object);

            //Assert    
            Assert.True(success);
        }
        [Fact]
        public void successful_adding_three_new_appointments()// ƒобавить тест на добавление нескольких записей в очередь
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.dentist;
            string name = "Insar";
            AppointmentMock1
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock1
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock1
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock1
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 1);
            hour = ReceptionHours.nine;
            doctor = Doctors.urologist;
            AppointmentMock2
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock2
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock2
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock2
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 11);
            hour = ReceptionHours.eleven;
            doctor = Doctors.otolaryngologist;
            AppointmentMock3
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock3
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock3
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock3
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            List<IAppointment> expectedAppointmentsList = new List<IAppointment>();
            expectedAppointmentsList.Add(AppointmentMock1.Object);
            expectedAppointmentsList.Add(AppointmentMock2.Object);
            expectedAppointmentsList.Add(AppointmentMock3.Object);

            //Act
            bool success1 = equeue.AddAppointment(AppointmentMock1.Object);
            bool success2 = equeue.AddAppointment(AppointmentMock2.Object);
            bool success3 = equeue.AddAppointment(AppointmentMock3.Object);

            //Assert    
            ListAppointmentsOutput resultNameAppointments = equeue.GetAppointmentsByName(name);
            bool realyExist = Matches(resultNameAppointments.appointments, expectedAppointmentsList);
            Assert.True(success1 & success2 & success3 & realyExist);
        }
        [Fact]
        public void unsuccessful_adding_one_appointment_which_alredy_exist()
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.dentist;
            AppointmentMock
                .Setup(x => x.GetName())
                .Returns("Insar");
            AppointmentMock
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            equeue.AddAppointment(AppointmentMock.Object);

            //Act
            bool success = equeue.AddAppointment(AppointmentMock.Object);

            //Assert    
            Assert.False(success);
        }
        [Fact]
        public void successful_deleting_one_exist_appointment()
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.dentist;
            AppointmentMock
                .Setup(x => x.GetName())
                .Returns("Victor");
            AppointmentMock
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            equeue.AddAppointment(AppointmentMock.Object);

            //Act
            bool success = equeue.DeleteAppointment(AppointmentMock.Object);

            //Assert    
            ListAppointmentsOutput resultNameAppointments = equeue.GetAppointmentsByName("Victor");
            Assert.True(success & !resultNameAppointments.searchResult); // ѕроверка того, что записть удалена из списка
        }
        [Fact]
        public void successful_deleting_tree_exist_appointments() // ”спешное удаление нескольких записей
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.dentist;
            string name = "Insar";
            AppointmentMock1
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock1
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock1
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock1
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 1);
            hour = ReceptionHours.nine;
            doctor = Doctors.urologist;
            AppointmentMock2
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock2
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock2
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock2
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 11);
            hour = ReceptionHours.eleven;
            doctor = Doctors.otolaryngologist;
            AppointmentMock3
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock3
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock3
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock3
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            equeue.AddAppointment(AppointmentMock1.Object);
            equeue.AddAppointment(AppointmentMock2.Object);
            equeue.AddAppointment(AppointmentMock3.Object);

            //Act
            bool success1 = equeue.DeleteAppointment(AppointmentMock1.Object);
            bool success2 = equeue.DeleteAppointment(AppointmentMock2.Object);
            bool success3 = equeue.DeleteAppointment(AppointmentMock3.Object);

            //Assert    
            ListAppointmentsOutput resultNameAppointments = equeue.GetAppointmentsByName(name);
            Assert.True(success1 & success2 & success3 & !resultNameAppointments.searchResult);
        }
        [Fact]
        public void unsuccessful_deleting_appointment_which_not_exists_in_queue() // ѕереименовать, чтобы было пон€тно (проверить во всех тестах)
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.dentist;
            AppointmentMock
                .Setup(x => x.GetName())
                .Returns("Insar");
            AppointmentMock
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            //Act
            bool success = equeue.DeleteAppointment(AppointmentMock.Object);

            //Assert
            
            Assert.False(success);
        }
        [Fact]
        public void successful_getting_list_appointments_by_name()
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.dentist;
            string name = "Insar";
            AppointmentMock1
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock1
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock1
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock1
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 1);
            hour = ReceptionHours.nine;
            doctor = Doctors.urologist;
            AppointmentMock2
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock2
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock2
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock2
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 11);
            hour = ReceptionHours.eleven;
            doctor = Doctors.otolaryngologist;
            name = "Oleg";
            AppointmentMock3
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock3
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock3
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock3
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            List<IAppointment> specificNameAppointments = new List<IAppointment>();
            specificNameAppointments.Add(AppointmentMock1.Object);
            specificNameAppointments.Add(AppointmentMock2.Object);

            equeue.AddAppointment(AppointmentMock1.Object);
            equeue.AddAppointment(AppointmentMock2.Object);
            equeue.AddAppointment(AppointmentMock3.Object);
            //Act
            ListAppointmentsOutput resultNameAppointments = equeue.GetAppointmentsByName("Insar");
            
            bool success = Matches(specificNameAppointments, resultNameAppointments.appointments);
            //Assert
            Assert.True(success);
        }
        [Fact]
        public void getting_similar_lists_appointments_by_similar_input_names_from_GetAppointmentsByName_2_times_in_a_row()
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.dentist;
            string name = "Insar";
            AppointmentMock1
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock1
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock1
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock1
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 1);
            hour = ReceptionHours.nine;
            doctor = Doctors.urologist;
            AppointmentMock2
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock2
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock2
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock2
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 11);
            hour = ReceptionHours.eleven;
            doctor = Doctors.otolaryngologist;
            name = "Oleg";
            AppointmentMock3
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock3
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock3
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock3
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            equeue.AddAppointment(AppointmentMock1.Object);
            equeue.AddAppointment(AppointmentMock2.Object);
            equeue.AddAppointment(AppointmentMock3.Object);

            //Act
            ListAppointmentsOutput firstNameAppointmentsList = equeue.GetAppointmentsByName("Insar");
            ListAppointmentsOutput secondNameAppointmentsList = equeue.GetAppointmentsByName("Insar");

            bool success = Matches(firstNameAppointmentsList.appointments, secondNameAppointmentsList.appointments);
            //Assert
            Assert.True(success);
        }
        [Fact]
        public void successful_getting_list_appointments_by_doctor_and_date()
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.dentist;
            string name = "Insar";
            AppointmentMock1
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock1
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock1
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock1
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 11);
            hour = ReceptionHours.nine;
            doctor = Doctors.dentist;
            name = "Victor";
            AppointmentMock2
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock2
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock2
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock2
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = DateTime.Today;
            hour = ReceptionHours.eleven;
            doctor = Doctors.dentist;
            name = "Oleg";
            AppointmentMock3
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock3
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock3
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock3
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            List<IAppointment> specificDoctorAndDateAppointments = new List<IAppointment>();
            specificDoctorAndDateAppointments.Add(AppointmentMock1.Object);
            specificDoctorAndDateAppointments.Add(AppointmentMock3.Object);

            equeue.AddAppointment(AppointmentMock1.Object);
            equeue.AddAppointment(AppointmentMock2.Object);
            equeue.AddAppointment(AppointmentMock3.Object);
            //Act
            ListAppointmentsOutput resultDoctorAndDateAppointments = equeue.GetAppointmentsByDoctorAndDate(doctor, date);

            //Assert
            bool success = Matches(specificDoctorAndDateAppointments, resultDoctorAndDateAppointments.appointments);
            Assert.True(success);
        }
        [Fact]
        public void getting_similar_lists_appointments_by_similar_input_doctor_and_date_from_GetAppointmentsByDoctorAndDate_2_times_in_a_row()
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.dentist;
            string name = "Insar";
            AppointmentMock1
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock1
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock1
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock1
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 11);
            hour = ReceptionHours.nine;
            doctor = Doctors.dentist;
            name = "Victor";
            AppointmentMock2
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock2
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock2
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock2
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = DateTime.Today;
            hour = ReceptionHours.eleven;
            doctor = Doctors.dentist;
            name = "Oleg";
            AppointmentMock3
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock3
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock3
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock3
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            equeue.AddAppointment(AppointmentMock1.Object);
            equeue.AddAppointment(AppointmentMock2.Object);
            equeue.AddAppointment(AppointmentMock3.Object);
            //Act
            ListAppointmentsOutput firstDoctorAndDateAppointmentsList = equeue.GetAppointmentsByDoctorAndDate(doctor, date);
            ListAppointmentsOutput secondDoctorAndDateAppointmentsList = equeue.GetAppointmentsByDoctorAndDate(doctor, date);
            //Assert
            bool success = Matches(firstDoctorAndDateAppointmentsList.appointments, secondDoctorAndDateAppointmentsList.appointments);
            Assert.True(success);
        }
        [Fact]
        public void successful_getting_list_appointments_by_date()
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.urologist;
            string name = "Insar";
            AppointmentMock1
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock1
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock1
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock1
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 11);
            hour = ReceptionHours.nine;
            doctor = Doctors.dentist;
            name = "Victor";
            AppointmentMock2
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock2
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock2
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock2
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = DateTime.Today;
            hour = ReceptionHours.eleven;
            doctor = Doctors.dentist;
            name = "Oleg";
            AppointmentMock3
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock3
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock3
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock3
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            List<IAppointment> specificDateAppointments = new List<IAppointment>();
            specificDateAppointments.Add(AppointmentMock1.Object);
            specificDateAppointments.Add(AppointmentMock3.Object);

            equeue.AddAppointment(AppointmentMock1.Object);
            equeue.AddAppointment(AppointmentMock2.Object);
            equeue.AddAppointment(AppointmentMock3.Object);

            //Act
            ListAppointmentsOutput resultDateAppointments = equeue.GetAppointmentsByDate(date);

            bool success = Matches(specificDateAppointments, resultDateAppointments.appointments);
            //Assert
            Assert.True(success);
        }
        [Fact]
        public void getting_similar_lists_appointments_by_similar_date_from_GetAppointmentsByDate_2_times_in_a_row()
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.urologist;
            string name = "Insar";
            AppointmentMock1
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock1
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock1
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock1
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = new DateTime(2020, 11, 11);
            hour = ReceptionHours.nine;
            doctor = Doctors.dentist;
            name = "Victor";
            AppointmentMock2
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock2
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock2
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock2
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = DateTime.Today;
            hour = ReceptionHours.eleven;
            doctor = Doctors.dentist;
            name = "Oleg";
            AppointmentMock3
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock3
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock3
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock3
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            equeue.AddAppointment(AppointmentMock1.Object);
            equeue.AddAppointment(AppointmentMock2.Object);
            equeue.AddAppointment(AppointmentMock3.Object);

            //Act
            ListAppointmentsOutput firstDateAppointmentsList = equeue.GetAppointmentsByDate(date);
            ListAppointmentsOutput secondDateAppointmentsList = equeue.GetAppointmentsByDate(date);

            bool success = Matches(firstDateAppointmentsList.appointments, secondDateAppointmentsList.appointments);
            //Assert
            Assert.True(success);
        }
        [Fact]
        public void successful_getting_list_free_times_by_doctor_and_date()
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.urologist;
            string name = "Insar";
            AppointmentMock1
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock1
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock1
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock1
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = DateTime.Today;
            hour = ReceptionHours.nine;
            doctor = Doctors.dentist;
            name = "Victor";
            AppointmentMock2
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock2
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock2
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock2
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = DateTime.Today;
            hour = ReceptionHours.eleven;
            doctor = Doctors.dentist;
            name = "Oleg";
            AppointmentMock3
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock3
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock3
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock3
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            List<ReceptionHours> freeTimes = new List<ReceptionHours>();
            freeTimes.Add(ReceptionHours.eight);
            freeTimes.Add(ReceptionHours.ten);
            freeTimes.Add(ReceptionHours.twelve);
            freeTimes.Add(ReceptionHours.fourteen);
            freeTimes.Add(ReceptionHours.fifteen);
            freeTimes.Add(ReceptionHours.sixteen);
            freeTimes.Add(ReceptionHours.seventeen);

            equeue.AddAppointment(AppointmentMock1.Object);
            equeue.AddAppointment(AppointmentMock2.Object);
            equeue.AddAppointment(AppointmentMock3.Object);

            //Act
            ListTimesOutput resultTimes = equeue.GetFreeTimesByDoctorAndDate(doctor, date);

            bool success = Matches(freeTimes, resultTimes.times);
            //Assert
            Assert.True(success);
        }
        [Fact]
        public void getting_similar_lists_free_times_by_similar_input_doctor_and_date_from_GetFreeTimesByDoctorAndDate_2_times_in_a_row()
        {
            // Arrange
            DateTime date = DateTime.Today;
            ReceptionHours hour = ReceptionHours.ten;
            Doctors doctor = Doctors.urologist;
            string name = "Insar";
            AppointmentMock1
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock1
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock1
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock1
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = DateTime.Today;
            hour = ReceptionHours.nine;
            doctor = Doctors.dentist;
            name = "Victor";
            AppointmentMock2
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock2
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock2
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock2
                .Setup(x => x.GetDoctor())
                .Returns(doctor);
            date = DateTime.Today;
            hour = ReceptionHours.eleven;
            doctor = Doctors.dentist;
            name = "Oleg";
            AppointmentMock3
                .Setup(x => x.GetName())
                .Returns(name);
            AppointmentMock3
                .Setup(x => x.GetDate())
                .Returns(date);
            AppointmentMock3
                .Setup(x => x.GetTime())
                .Returns(hour);
            AppointmentMock3
                .Setup(x => x.GetDoctor())
                .Returns(doctor);

            equeue.AddAppointment(AppointmentMock1.Object);
            equeue.AddAppointment(AppointmentMock2.Object);
            equeue.AddAppointment(AppointmentMock3.Object);

            //Act
            ListTimesOutput firstResultTimesList = equeue.GetFreeTimesByDoctorAndDate(doctor, date);
            ListTimesOutput secondResultTimesList = equeue.GetFreeTimesByDoctorAndDate(doctor, date);

            bool success = Matches(firstResultTimesList.times, secondResultTimesList.times);
            //Assert
            Assert.True(success);
        }
    } // ѕолучение записей с одинаковыми данными несколько раз подр€д 
    // ¬ыход при пустом выходном списке подумать
}