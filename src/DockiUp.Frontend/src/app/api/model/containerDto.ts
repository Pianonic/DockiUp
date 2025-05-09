/**
 * DockiUp.API
 *
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { StatusType } from './statusType';
import { UpdateMethod } from './updateMethod';
import { WebhookSecret } from './webhookSecret';


export interface ContainerDto { 
    dbContainerId?: number;
    containerId?: string | null;
    name: string | null;
    description: string | null;
    gitUrl: string | null;
    path: string | null;
    lastUpdated: string;
    lastGitPush: string;
    status: StatusType;
    updateMethod: UpdateMethod;
    webhookSecret?: WebhookSecret;
    checkIntervals?: number | null;
}
export namespace ContainerDto {
}


