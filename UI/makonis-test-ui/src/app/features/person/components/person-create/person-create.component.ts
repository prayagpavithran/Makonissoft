import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { CommonResponse } from '../../models/common-response';
import { Person } from '../../models/person';
import { PersonService } from '../../services/person.service';

@Component({
  selector: 'app-person-create',
  templateUrl: './person-create.component.html',
  styleUrls: ['./person-create.component.css']
})
export class PersonCreateComponent implements OnInit {

  personForm: FormGroup = this.fb.group({});
  private unsubscribe$ = new Subject();
  
  constructor(
      private fb: FormBuilder,
      private personService: PersonService
    ) { 

    }

  ngOnInit(): void {
    this.buildPersonCreateForm();
  }

  ngOnDestroy() {
    this.unsubscribe$.next(true);
    // Now let's also unsubscribe from the subject itself:
    this.unsubscribe$.unsubscribe();
  }

  onPersonSave(){
    if(this.personForm.valid){
      const personInfo = {
        firstName : this.personForm.get("firstName")?.value,
        lastName  : this.personForm.get("lastName")?.value
      } as Person;

      this.personService.createPerson(personInfo).pipe(takeUntil(this.unsubscribe$)).subscribe((result:CommonResponse)=>{
        if(result.isSuccess){
          alert("Person details has been saved successfully.")
        }
        else{
          alert("Error occured while saving person information.")
        }
      })
    }
  }

  buildPersonCreateForm(){
    this.personForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]]      
    });
  }


}
