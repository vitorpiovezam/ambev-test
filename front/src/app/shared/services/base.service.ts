import { HttpClient } from "@angular/common/http";
import { BaseModel } from "../models/base.model";

export abstract class BaseService<T extends BaseModel> {
    apiUrl = 'http://localhost:8080';

    constructor(http: HttpClient) { }
}
