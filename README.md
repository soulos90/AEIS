# AEIS built from CA state template MVC

Accessing Data:
	Data is manipulated through object of type StateTemplateV5Beta.Models."Answer/Question/User"().
	
	The database is manipulated through object of type StateTemplateV5Beta.Controllers."Answers/Questions/Users"Controller()
	
	retrieving data:
		StateTemplateV5Beta.Models."Answer/Question/User" = StateTemplateV5Beta.Controllers."Answers/Questions/Users"Controller().Get"Q/U/A"("int/id/id")
			id is created by new{string email,int iterator}
				#id is created by StateTemplateV5Beta.Controllers."Answers/Questions/Users"Controller.Next(string email)
		access StateTemplateV5Beta.Models."Answer/Question/User"'s datamembers	
	
	inserting data:
		create new data:
			populate StateTemplateV5Beta.Models."Answer/User"'s datamembers
			StateTemplateV5Beta.Controllers."Answers/Users"Controller().Post"User/Answer"(StateTemplateV5Beta.Models."Answer/User")
				
		modify preexisting data:
			populate StateTemplateV5Beta.Models."Answer/User"'s datamembers
			StateTemplateV5Beta.Controllers."Answers/Users"Controller().Put"User/Answer"("id/id",StateTemplateV5Beta.Models."Answer/User")
				id is created by new{string email,int iterator}
					iterator is created by StateTemplateV5Beta.Controllers."Answers/Questions/Users"Controller.Next(string email)

The California State Template is a .NET MVC template and website standard offered by the California Department of Technology to state agencies and departments within the State of California and beyond. Please visit webtools.ca.gov for more information.

This is a Beta release which means it is a pre-release of software that is given out to a large group of users to try under real conditions. Beta versions have gone through alpha testing inhouse and are generally fairly close in look, feel and function to the final product; however, design changes often occur as a result. 

We encourage you to utilize GitHub to provide feedback and collaborate on the development of the next California state template! For questions please email Info.Eservices@state.ca.gov