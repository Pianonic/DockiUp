/**
 * DockiUp.API
 *
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { UpdateMethod } from './updateMethod';


export interface CreateContainerDto { 
    containerName: string | null;
    gitUrl: string | null;
    description?: string | null;
    updateMethod: UpdateMethod;
    checkIntervals?: number | null;
}
export namespace CreateContainerDto {
}


