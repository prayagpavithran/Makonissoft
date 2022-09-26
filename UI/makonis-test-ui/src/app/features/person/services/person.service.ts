import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Person } from '../models/person';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CommonResponse } from '../models/common-response';

@Injectable()
export class PersonService {

  constructor(private http: HttpClient) { }

  createPerson(person: Person): Observable<CommonResponse> {
    return this.http.post<CommonResponse>(`${environment.apiUrl}/persons`, person);
  }
}
